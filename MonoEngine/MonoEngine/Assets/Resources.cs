using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MonoEngine.Render;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoEngine.Game;
using MonoEngine.Physics;
using MonoEngine.Physics.Physics2D;
using MonoEngine.Physics.Physics3D;
using MonoEngine.UI;
using MonoEngine.Assets;
using System.Reflection;
using System.Text;
using System.Collections;

namespace MonoEngine.Assets
{
    public class Resources
    {
        // SERIALISATION -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // & deserialisation (but don't tell .Net)
        // Here I need to recover Monogame's stupidity, and make sure that my custom serialisation can serialise their unserialisable structs and classes (like Vector3, Matrix, everything, essentially)
        // Which I will do by quietly doing a bunch of work that no one will see, and it will be great.
        // Time to begin
        // It turned out to be completely okay, mostly simple, and quite delicious.

        /// <summary>
        /// Pass an object to this, and it will try to put it in the appropriate folder as a .prefab (This means it must be a Resources managed asset)
        /// </summary>
        /// <param name="obj">The Object to be serialised</param>
        public static void Serialise(object obj)
        {
            Type type = obj.GetType();

            FieldInfo[] info_fs = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] info_ps = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            string name = "";

            foreach (FieldInfo info in info_fs)
            {
                if (info.Name.ToLower() == "name")
                {
                    name = info.GetValue(obj) as string;
                }
            }

            foreach (PropertyInfo info in info_ps)
            {
                if (info.Name.ToLower() == "name")
                {
                    name = info.GetValue(obj) as string;
                }
            }

            Type check = new object().GetType();
            Type t = type;
            Type baseType = t.BaseType;
            while (baseType != check)
            {
                t = type.BaseType;
                baseType = t.BaseType;
            }

            XmlWriter writer;
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
            };

            using (writer = XmlWriter.Create("Content/" + instance.resourceManagers[t].Path + "/" + name + ".prefab", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(type.FullName);
                writer.WriteAttributeString("name", name);

                foreach (FieldInfo info in info_fs)
                {
                    writer.WriteStartElement(info.Name);
                    writer.WriteAttributeString("type", info.FieldType.ToString());
                    writer.WriteAttributeString("field", "true");

                    object val = info.GetValue(obj);
                    if (val == null)
                    {
                        writer.WriteString("");
                    }
                    else
                    {
                        // Here I need to check if the value is enumeratable, thereby, it needs to be iterated through, unless it is a string (cus, no)
                        if (val is IEnumerable && !(val is string))
                        {
                            foreach (object o in (info.GetValue(obj) as IEnumerable))
                            {
                                writer.WriteStartElement("ref");
                                writer.WriteAttributeString("type", o.GetType().ToString());
                                writer.WriteRaw(o.ToString());
                                writer.WriteEndElement();

                                if (!o.GetType().IsValueType && o.GetType().IsClass)
                                {
                                    Serialise(o);
                                }
                            }
                        }
                        else
                        {
                            writer.WriteRaw(val.ToString());
                        }
                    }
                    writer.WriteEndElement();
                }

                foreach (PropertyInfo info in info_ps)
                {
                    writer.WriteStartElement(info.Name);
                    writer.WriteAttributeString("type", info.PropertyType.ToString());
                    writer.WriteAttributeString("field", "false");

                    object val = info.GetValue(obj);
                    if (val == null)
                    {
                        writer.WriteString("");
                    }
                    else
                    {
                        // Here I need to check if the value is enumeratable, thereby, it needs to be iterated through, unless it is a string (cus, no)
                        if (val is IEnumerable && !(val is string))
                        {
                            foreach (object o in (info.GetValue(obj) as IEnumerable))
                            {
                                writer.WriteStartElement("ref");
                                writer.WriteAttributeString("type", o.GetType().ToString());
                                writer.WriteRaw(o.ToString());
                                writer.WriteEndElement();

                                if (!o.GetType().IsValueType && o.GetType().IsClass)
                                {
                                    Serialise(o);
                                }
                            }
                        }
                        else
                        {
                            writer.WriteRaw(val.ToString());
                        }
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public static object Deserialise(string path)
        {
            Type obj_type = null;
            object obj = null;
            FieldInfo[] obj_info_fields = null;
            PropertyInfo[] obj_info_properties = null;

            using (XmlReader reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "XmlDocument":
                                // Sweetheart I really don't care.
                                break;
                            default:
                                if (obj == null)
                                {
                                    // The object hasn't been made, which means I can assume this element is telling me what it is
                                    obj_type = Type.GetType(reader.Name);
                                    obj_info_fields = obj_type.GetFields(BindingFlags.Instance | BindingFlags.Public);
                                    obj_info_properties = obj_type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                                    //obj = obj_type.GetConstructor(Type.EmptyTypes).Invoke();
                                    obj = Activator.CreateInstance(obj_type);

                                    Type check = new object().GetType();
                                    Type t = obj_type;
                                    Type baseType = t.BaseType;
                                    while (baseType != check)
                                    {
                                        t = t.BaseType;
                                        baseType = t.BaseType;
                                    }
                                    instance.resourceManagers[t].AddResource(reader["name"], obj);
                                }
                                else
                                {
                                    // The object already exists, use reflection to give it it's values, coming from the element
                                    // Check if the element is a field, if it isn't then it is a property
                                    if (bool.Parse(reader["field"]))
                                    {
                                        // Go through all the fields to find the one that this element is changing
                                        foreach (FieldInfo info in obj_info_fields)
                                        {
                                            if (info.Name == reader.Name)
                                            {
                                                // We found it boys
                                                // Change its value
                                                Type field_type = Type.GetType(reader["type"]);

                                                int depth = reader.Depth;
                                                int depth_two = reader.Depth;

                                                reader.Read();

                                                switch (field_type.FullName)
                                                {
                                                    case "MonoEngine.Game.GameObject":
                                                        if (reader.Value != "")
                                                            info.SetValue(obj, Ref(field_type, reader.Value));
                                                        else
                                                            info.SetValue(obj, null);
                                                        break;
                                                    case "MonoEngine.Physics.PhysicsMaterial":
                                                        // I need to make a physics material here
                                                        PhysicsMaterial mat = new PhysicsMaterial();
                                                        depth_two = reader.Depth;
                                                        while (reader.Depth >= depth_two)
                                                        {
                                                            if (reader.IsStartElement())
                                                            {
                                                                switch (reader.Name)
                                                                {
                                                                    case "Density":
                                                                        //Density
                                                                        reader.Read();
                                                                        mat.Density = float.Parse(reader.Value);
                                                                        break;
                                                                    case "Friction":
                                                                        //Friction
                                                                        reader.Read();
                                                                        mat.Friction = float.Parse(reader.Value);
                                                                        break;
                                                                    case "Restitution":
                                                                        //Restitution
                                                                        reader.Read();
                                                                        mat.Restitution = float.Parse(reader.Value);
                                                                        break;
                                                                }
                                                            }

                                                            reader.Read();
                                                        }

                                                        break;
                                                    case "MonoEngine.Transform":
                                                        // {M11:1 M12:0 M13:0 M14:0} {M21:0 M22:1 M23:0 M24:0} {M31:0 M32:0 M33:1 M34:0} {M41:0 M42:0 M43:0 M44:1}
                                                        string cleanString = reader.Value;
                                                        cleanString = cleanString.Replace("{", "");
                                                        cleanString = cleanString.Replace("}", "");
                                                        // M11:1 M12:0 M13:0 M14:0} {M21:0 M22:1 M23:0 M24:0} {M31:0 M32:0 M33:1 M34:0} {M41:0 M42:0 M43:0 M44:1
                                                        for (int i = 0; i < 4; i++)
                                                        {
                                                            for (int j = 0; j < 4; j++)
                                                            {
                                                                cleanString = cleanString.Replace("M" + (i + 1).ToString() + (j + 1).ToString() + ":", "");
                                                            }
                                                        }
                                                        // 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1
                                                        // 1   0   0   0   0   1   0   0   0   0   1   0   0   0   0   1
                                                        // M11 M12 M13 M14 M21 M22 M23 M24 M31 M32 M33 M34 M41 M42 M43 M44
                                                        // [0] [1] [2] [3] [4] [5] [6] [7] [8] [9] [10][11][12][13][14][15]
                                                        string[] vals = cleanString.Split(' ');
                                                        // Well this looks ridiculous
                                                        info.SetValue(obj, new Transform(new Matrix(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]), float.Parse(vals[3]),
                                                                    float.Parse(vals[4]), float.Parse(vals[5]), float.Parse(vals[6]), float.Parse(vals[7]),
                                                                    float.Parse(vals[8]), float.Parse(vals[9]), float.Parse(vals[10]), float.Parse(vals[11]),
                                                                    float.Parse(vals[12]), float.Parse(vals[13]), float.Parse(vals[14]), float.Parse(vals[15]))));
                                                        break;
                                                    case "MonoEngine.Shapes.Shape":
                                                        List<Vector3> vectors = new List<Vector3>();
                                                        depth_two = reader.Depth;
                                                        while (reader.Depth >= depth_two)
                                                        {
                                                            if (reader.IsStartElement())
                                                            {
                                                                switch (reader.Name)
                                                                {
                                                                    case "Vector3":
                                                                        vectors.Add(Helper_Vector3FromXmlReader(reader));
                                                                        break;
                                                                }
                                                            }

                                                            reader.Read();
                                                        }
                                                        // Circle
                                                        if (vectors.Count == 1)
                                                            info.SetValue(obj, new Shapes.Circle(obj_type.GetField("transform", BindingFlags.Public | BindingFlags.Instance).GetValue(obj) as Transform, vectors[0]));
                                                        // AABB
                                                        else
                                                            info.SetValue(obj, new Shapes.AABB(obj_type.GetField("transform", BindingFlags.Public | BindingFlags.Instance).GetValue(obj) as Transform, vectors[0], vectors[1]));
                                                        break;
                                                    default:
                                                        if (typeof(IEnumerable).IsAssignableFrom(field_type) && !(typeof(string).IsAssignableFrom(field_type)))
                                                        {
                                                            while (reader.Read() && reader.Depth > depth)
                                                            {
                                                                if (reader.IsStartElement())
                                                                {
                                                                    Type enum_type = Type.GetType(reader["type"]);
                                                                    Type check = new object().GetType();
                                                                    Type t = enum_type;
                                                                    Type baseType = t.BaseType;
                                                                    while (baseType != check)
                                                                    {
                                                                        t = t.BaseType;
                                                                        baseType = t.BaseType;
                                                                    }

                                                                    reader.Read();
                                                                    info.SetValue(obj, Activator.CreateInstance(field_type));
                                                                    field_type.GetMethod("Add").Invoke(info.GetValue(obj), new object[] { Helper_GetValueSafe(t, reader.Value, reader) });
                                                                }
                                                            }
                                                        }
                                                        else if (typeof(Enum).IsAssignableFrom(field_type))
                                                        {
                                                            Type enum_type = Type.GetType(field_type.FullName);
                                                            info.SetValue(obj, Enum.Parse(enum_type, reader.Value));
                                                        }
                                                        else
                                                        {
                                                            info.SetValue(obj, Convert.ChangeType(reader.Value, field_type));
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            return obj;
        }

        private static object Helper_GetValueSafe(Type info, string value, XmlReader reader)
        {
            int depth = reader.Depth;

            switch (info.FullName)
            {
                case "MonoEngine.Game.GameObject":
                    if (value != "")
                        return Ref(info, value);
                    else
                        return null;
                default:
                    //if (info is IEnumerable)
                    //{
                    //    while (reader.Read() && reader.Depth > depth)
                    //    {
                    //        if (reader.IsStartElement())
                    //        {
                    //            type.GetMethod("Add").Invoke(obj, );
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    return Convert.ChangeType(value, info);
                    //}
            }
        }

        private static Vector3 Helper_Vector3FromXmlReader(XmlReader reader)
        {
            Vector3 v = Vector3.Zero;

            reader.Read();

            //{X:-0.5 Y:0 Z:-0.5}
            string cleanString = reader.Value;
            //X:-0.5 Y:0 Z:-0.5}
            cleanString = cleanString.Replace("{", "");
            //X:-0.5 Y:0 Z:-0.5
            cleanString = cleanString.Replace("}", "");
            //-0.5 Y:0 Z:-0.5
            cleanString = cleanString.Replace("X:", "");
            //-0.5 0 Z:-0.5
            cleanString = cleanString.Replace("Y:", "");
            //-0.5 0 -0.5
            cleanString = cleanString.Replace("Z:", "");
            //-0.5 0 -0.5
            //-0.5  0 -0.5
            //  X   Y   Z
            // [0] [1] [2]
            string[] vals = cleanString.Split(' ');

            v = new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]));

            return v;
        }

        private static Matrix Helper_MatrixFromXmlReader(XmlReader reader)
        {
            // {M11:1 M12:0 M13:0 M14:0} {M21:0 M22:1 M23:0 M24:0} {M31:0 M32:0 M33:1 M34:0} {M41:0 M42:0 M43:0 M44:1}
            string cleanString = reader.Value;
            cleanString = cleanString.Replace("{", "");
            cleanString = cleanString.Replace("}", "");
            // M11:1 M12:0 M13:0 M14:0} {M21:0 M22:1 M23:0 M24:0} {M31:0 M32:0 M33:1 M34:0} {M41:0 M42:0 M43:0 M44:1
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    cleanString = cleanString.Replace("M" + (i + 1).ToString() + (j + 1).ToString() + ":", "");
                }
            }
            // 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1
            // 1   0   0   0   0   1   0   0   0   0   1   0   0   0   0   1
            // M11 M12 M13 M14 M21 M22 M23 M24 M31 M32 M33 M34 M41 M42 M43 M44
            // [0] [1] [2] [3] [4] [5] [6] [7] [8] [9] [10][11][12][13][14][15]
            string[] vals = cleanString.Split(' ');
            // Well this looks ridiculous
            return new Matrix(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]), float.Parse(vals[3]),
                float.Parse(vals[4]), float.Parse(vals[5]), float.Parse(vals[6]), float.Parse(vals[7]),
                float.Parse(vals[8]), float.Parse(vals[9]), float.Parse(vals[10]), float.Parse(vals[11]),
                float.Parse(vals[12]), float.Parse(vals[13]), float.Parse(vals[14]), float.Parse(vals[15]));
        }

        public static object Ref(XmlReader reader)
        {
            // This will be called whenever a reader has found a <ref> element and should do whatever is appropriate to make that <ref> into an object
            Type type = null;

            type = Type.GetType(reader["type"], true, true);

            if (reader.Value != null)
                return LoadAsset(type, reader.Value, SceneManager.activeScene);
            else
                return null;
        }

        public static object Ref(Type type, string name)
        {
            // This will be called whenever a reader has found a <ref> element and should do whatever is appropriate to make that <ref> into an object
            return LoadAsset(type, name, SceneManager.activeScene);
        }

        public static object Find(XmlReader reader)
        {
            Type type = null;

            type = Type.GetType(reader["type"], true, true);

            if (reader.Read())
            {
                return instance.resourceManagers[type].GetResource(reader.Value);
            }

            return null;
        }

        public static object Make(XmlReader reader)
        {


            return null;
        }

        private static Resources instance;
        private Resources()
        {
            resourceManagers = new Dictionary<Type, ResourceManager>();

            Type type = new GameObject().GetType();
            LoaderGameObject loaderGO = new LoaderGameObject(type);
            resourceManagers.Add(type, new ResourceManager("Assets/Objects", type, loaderGO));

            type = new ModelWrapper().GetType();
            LoaderModel loaderM = new LoaderModel(type);
            resourceManagers.Add(type, new ResourceManager("Assets/Models", type, loaderM));

            type = new Font().GetType();
            LoaderFont loaderF = new LoaderFont(type);
            resourceManagers.Add(type, new ResourceManager("Assets/Fonts", type, loaderF));

            type = new Sprite().GetType();
            LoaderSprite loaderS = new LoaderSprite(typeof(Sprite));
            resourceManagers.Add(type, new ResourceManager("Assets/Textures", type, loaderS));

            type = new RenderTarget2DWrapper().GetType();
            resourceManager_renderTarget2D = new ResourceManager("Assets/Shaders", type, loaderS);
            resourceManagers.Add(type, resourceManager_renderTarget2D);
        }

        public static Resources Initialise()
        {
            instance = (instance == null) ? new Resources() : instance;
            return instance;
        }

        // The strange dictionary of all resource managers, keyed by Type
        private Dictionary<Type, ResourceManager> resourceManagers;
        private ResourceManager resourceManager_renderTarget2D;

        public static bool CheckForAsset(string name, Type type)
        {
            return instance.resourceManagers[type].ContainsResource(name);
        }

        public static void RemoveAsset(string name, Type type)
        {
            instance.resourceManagers[type].RemoveResource(name);
        }

        public static object LoadAsset(Type type, string name, Scene parent)
        {
            ResourceManager manager = instance.resourceManagers[type];

            if (manager.ContainsResource(name))
            {
                return manager.GetResource(name);
            }
            else
            {
                object asset = manager.Loader.LoadAsset(manager.Path, name, parent);

                // Deserialisation needs to add objects itself, sometimes, to avoid StackOverflow, and so sometimes
                // after LoadAsset (which can cause deserialisation) it needs to verify again if the object truly still isn't in the dictionary
                if (!manager.ContainsResource(name))
                    instance.resourceManagers[type].AddResource(name, asset);

                return asset;
            }
        }

        public static RenderTarget2DWrapper GetRenderTarget2D(string name)
        {

            if (instance.resourceManager_renderTarget2D.ContainsResource(name))
            {
                return instance.resourceManager_renderTarget2D.GetResource(name) as RenderTarget2DWrapper;
            }
            else
            {
                return null;
            }
        }

        public static RenderTarget2DWrapper LoadRenderTarget2D(string name, Scene parent, int width, int height, bool mipMap, SurfaceFormat surfaceFormat, DepthFormat depthFormat, int multiSampleCount, RenderTargetUsage usage)
        {
            RenderTarget2DWrapper target = new RenderTarget2DWrapper(new RenderTarget2D(GraphicsHelper.graphicsDevice, width, height, mipMap, surfaceFormat, depthFormat, multiSampleCount, usage));
            ResourceManager manager = instance.resourceManagers[target.GetType()];

            if (manager.ContainsResource(name))
            {
                if (target != manager.GetResource(name))
                {
                    throw new AssetExceptions.AssetAlreadyExists("Cannot resolve difference between existing RenderTarget2D and new RenderTarget2D\nExisting RenderTarget2D: " + instance.resourceManager_renderTarget2D.GetResource(name) + "\nNew RenderTarget2D" + target);
                }
            }
            else
            {
                manager.AddResource(name, target);
                RenderTargetBatch batch = new RenderTargetBatch(name, target);
                RenderManager.AddRenderTargetBatch(batch);
                return target;
            }

            if (parent != null)
            {
                parent.assets.assets[new RenderTarget2DWrapper().GetType()].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            return manager.GetResource(name) as RenderTarget2DWrapper;
        }

        public static void UnloadScene(Scene oldScene)
        {
            if (oldScene != null)
            {
                SceneAssetsPackage difference = oldScene.assets.Difference(SceneManager.activeScene.assets);

                // By finding the difference between the current scene and the newScene I am given a list of all the assets only found in the current scene,
                // which must all be unloaded, as they will no longer be used

                foreach (KeyValuePair<Type, List<string>> assets in difference.assets)
                {
                    foreach (string asset in assets.Value)
                    {
                        instance.resourceManagers[assets.Key].RemoveResource(asset);
                    }
                }
            }
        }

        public static void AddResourceManager(string path, Type type, ResourceManagerLoader loader)
        {
            if (!instance.resourceManagers.ContainsKey(type))
            {
                instance.resourceManagers.Add(type, new ResourceManager(path, type, loader));
            }
            else
            {
                throw new AssetExceptions.ResourceManagerOfTypeAlreadyExists("Cannot create ResourceManager of type " + type + " with path " + path + " as a ResourceManager of that type already exists");
            }
        }

        public static void RemoveResourceManager<T>()
        {
            if (!instance.resourceManagers.ContainsKey(typeof(T)))
            {
                instance.resourceManagers.Remove(typeof(T));
            }
        }
    }
}