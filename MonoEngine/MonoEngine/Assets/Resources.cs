using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MonoEngine.Render;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Game;
using MonoEngine.Physics;
using MonoEngine.Physics.Physics2D;
using MonoEngine.Physics.Physics3D;
using MonoEngine.UI;

namespace MonoEngine.Assets
{
    public class Resources
    {
        /// <summary>
        /// An enumerator for all Types of Resources
        /// </summary>
        public enum TypeOfRef { Camera, GameObject, Model, ModelRenderer, PhysicsBody2D, PhysicsMaterial, SpriteRenderer, TextRenderer }

        public static object Ref(XmlReader reader, out TypeOfRef type)
        {
            // This will be called whenever a reader has found a <ref> element and should do whatever is appropriate to make that <ref> into an object

            Enum.TryParse(reader["type"], out type);

            switch (type)
            {
                case TypeOfRef.Camera:
                    return instance.MakeCamera(reader);
                case TypeOfRef.GameObject:
                    return instance.LoadGameObject(reader);
                case TypeOfRef.Model:
                    return instance.MakeModel(reader);
                case TypeOfRef.ModelRenderer:
                    return instance.MakeModelRenderer(reader);
                case TypeOfRef.PhysicsBody2D:
                    return null;
                case TypeOfRef.PhysicsMaterial:
                    return null;
                case TypeOfRef.SpriteRenderer:
                    return null;
                case TypeOfRef.TextRenderer:
                    return null;
                default:
                    return null;
            }
        }

        public static object Find(XmlReader reader, out TypeOfRef type)
        {
            Enum.TryParse(reader["type"], out type);

            return null;
        }

        public static object Make(XmlReader reader, out TypeOfRef type)
        {
            Enum.TryParse(reader["type"], out type);

            return null;
        }

        private Camera LoadCamera(XmlReader reader)
        {
            return null;
        }

        private GameObject LoadGameObject(XmlReader reader)
        {
            GameObject gameObject = null;
            int depth = reader.Depth;

            while (reader.Read() && depth < reader.Depth)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "name":
                            if (resourceManagers[typeof(GameObject)].ContainsResource(reader.Value))
                                gameObject = (resourceManagers[typeof(GameObject)] as ResourceManager<GameObject>).GetResource(reader.Value);
                            else
                                gameObject = new GameObject(reader.Value);
                            break;
                        case "transform":
                            // This is an optional xml node
                            gameObject.transform = Transform.XmlToTransform(reader);
                            break;
                        case "ref":
                            TypeOfRef type;
                            object obj = Ref(reader, out type);
                            AttachRefToGameObject(type, gameObject, obj);
                            break;
                    }
                }
            }

            return gameObject;
        }

        private void AttachRefToGameObject(TypeOfRef type, GameObject gameObject, object obj)
        {
            switch (type)
            {
                case TypeOfRef.Camera:
                    gameObject.AddComponent(obj as Camera);
                    break;
                case TypeOfRef.GameObject:
                    gameObject.AddComponent(obj as GameObject);
                    break;
                case TypeOfRef.Model:
                    // Log a warning that GameObjects don't support having raw models as components, instead make it a model renderer
                    break;
                case TypeOfRef.ModelRenderer:
                    gameObject.AddComponent(obj as ModelRenderer);
                    break;
                case TypeOfRef.PhysicsBody2D:
                    gameObject.AddComponent(obj as PhysicsBody2D);
                    break;
                case TypeOfRef.PhysicsMaterial:
                    // Log a warning that GameObjects don't support having a physics material as a component
                    break;
                case TypeOfRef.SpriteRenderer:
                    gameObject.AddComponent(obj as SpriteRenderer);
                    break;
                case TypeOfRef.TextRenderer:
                    gameObject.AddComponent(obj as TextRenderer);
                    break;
            }
        }

        private Model LoadModel(XmlReader reader)
        {
            return null;
        }

        private ModelRenderer LoadModelRenderer(XmlReader reader)
        {
            return null;
        }

        private PhysicsBody2D LoadPhysicsBody2D(XmlReader reader)
        {
            return null;
        }

        private PhysicsMaterial LoadPhysicsMaterial(XmlReader reader)
        {
            return null;
        }

        private SpriteRenderer LoadSpriteRenderer(XmlReader reader)
        {
            return null;
        }

        private UIObject LoadUIObject(XmlReader reader)
        {
            // Take the reader, and make a UIObject from it
            return null;
        }

        private static Resources instance;
        private Resources()
        {
            resourceManagers = new Dictionary<Type, ResourceMetaData>();
            resourceManagers.Add(typeof(GameObject), new ResourceManager<GameObject>("Assets/Objects"));
            resourceManagers.Add(typeof(Model), new ResourceManager<GameObject>("Assets/Models"));
            resourceManagers.Add(typeof(Font), new ResourceManager<GameObject>("Assets/Fonts"));
            resourceManagers.Add(typeof(Texture2D), new ResourceManager<GameObject>("Assets/Textures"));
            resourceManagers.Add(typeof(RenderTarget2D), new ResourceManager<GameObject>("Assets/Shaders"));
            resourceManagers.Add(typeof(UIObject), new ResourceManager<UIObject>("Assets/UI"));
        }

        public static Resources Initialise()
        {
            instance = (instance == null) ? new Resources() : instance;
            return instance;
        }

        // The strange dictionary of all resource managers
        private Dictionary<Type, ResourceMetaData> resourceManagers;

        public static bool CheckForAsset<Type>(string name)
        {
            return ((ResourceManager<Type>)instance.resourceManagers[typeof(Type)]).ContainsResource(name);
        }

        public static RenderTarget2D GetRenderTarget2D(string name)
        {
            if (instance.resourceManagers[typeof(RenderTarget2D)].ContainsResource(name))
            {
                return (instance.resourceManagers[typeof(RenderTarget2D)] as ResourceManager<RenderTarget2D>).GetResource(name);
            }
            else
            {
                return null;
            }
        }

        public static GameObject LoadGameObject(string name, Scene parent)
        {
            // Load a resource from a file at the path + name.xnb pathway

            // If the model isn't already loaded (it's key isn't found in the dictionary)
            if (!instance.resourceManagers[typeof(GameObject)].ContainsResource(name))
            {
                // Needs to try to get the model at that name in the models path & load it
                GameObject asset = null;
                using (XmlReader reader = XmlReader.Create(@"./Content/" + instance.resourceManagers[typeof(GameObject)].Path + name + ".xml"))
                {
                    asset = instance.LoadGameObject(reader);
                }
                // add the model into the dictionary
                (instance.resourceManagers[typeof(GameObject)] as ResourceManager<GameObject>).AddResource(name, asset);
                return asset;
            }

            if (parent != null)
            {
                parent.assets.AddAsset(name, instance.resourceManagers[typeof(GameObject)].Path);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            // Pass the texture at that key in the dictionary
            return (instance.resourceManagers[typeof(GameObject)] as ResourceManager<GameObject>).GetResource(name);
        }

        public static Font LoadFont(string name, Scene parent)
        {
            // Load a font from a name at the Assets/Fonts/ + name#.xnb pathway (plus all it's sizes)

            string[] paths = Directory.GetFiles(@".\Content\Assets\Fonts\", name + "_*.xnb");

            char[] delimiters = { '.', '\\', '_' };

            List<int> sizes = null;
            int size = 0;
            List<SpriteFont> fonts = new List<SpriteFont>();
            foreach (string path in paths)
            {
                string[] split = path.Split(delimiters);
                // path looks like this .\\Content\\Assets\\Fonts\\name_*.xnb
                // delimited it becomes split[0] = "" | split[1] = "" | split[2] = "Content" | split[3] = "Assets" | split[4] = "Fonts" | split[5] = "name" | split[6] = "*" | split[7] = "xnb"

                if (split[6] != null && int.TryParse(split[6], out size))
                {
                    if (sizes == null)
                    {
                        sizes = new List<int>();
                        sizes.Add(size);
                        fonts.Add(ContentHelper.Content.Load<SpriteFont>("Assets/Fonts/" + name + "_" + split[6]));
                    }
                    else
                    {
                        for (int i = 0; i < sizes.Count; i++)
                        {
                            if (size < sizes[i])
                            {
                                sizes.Insert(i, size);
                                fonts.Insert(i, ContentHelper.Content.Load<SpriteFont>("Assets/Fonts/" + name + "_" + split[6]));
                            }
                        }
                    }
                }
            }

            if (parent != null)
            {
                parent.assets.assets["Assets/Fonts"].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            return new Font(sizes.ToArray(), fonts.ToArray(), name);
        }

        public static Model LoadModel(string name, Scene parent)
        {
            // Load a model from a name at the Assets/Models/ + name.xnb pathway

            // If the model isn't already loaded (it's key isn't found in the dictionary)
            if (!instance.models.ContainsKey(name))
            {
                // Needs to try to get the model at that name in the models path & load it
                Model model = ContentHelper.Content.Load<Model>("Assets/Models/" + name);
                // add the model into the dictionary
                instance.models.Add(name, model);
            }

            if (parent != null)
            {
                parent.assets.assets["Assets/Models"].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            // Pass the model at that key in the dictionary
            return instance.models[name];
        }

        public static Texture2D LoadTexture2D(string name, Scene parent)
        {
            // Load a texture from a texture at the Assets/Textures/ + name.xnb pathway

            // If the model isn't already loaded (it's key isn't found in the dictionary)
            if (!instance.texture2Ds.ContainsKey(name))
            {
                // Needs to try to get the model at that name in the models path & load it
                Texture2D texture = ContentHelper.Content.Load<Texture2D>("Assets/Textures/" + name);
                // add the model into the dictionary
                instance.texture2Ds.Add(name, texture);
            }

            if (parent != null)
            {
                parent.assets.assets["Assets/Textures"].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            // Pass the texture at that key in the dictionary
            return instance.texture2Ds[name];
        }

        public static RenderTarget2D LoadRenderTarget2D(string name, Scene parent, int width, int height, bool mipMap, SurfaceFormat surfaceFormat, DepthFormat depthFormat, int multiSampleCount, RenderTargetUsage usage)
        {
            RenderTarget2D target = new RenderTarget2D(GraphicsHelper.graphicsDevice, width, height, mipMap, surfaceFormat, depthFormat, multiSampleCount, usage);

            if (instance.renderTarget2Ds.ContainsKey(name))
            {
                if (target != instance.renderTarget2Ds[name])
                {
                    throw new AssetExceptions.AssetAlreadyExists("Cannot resolve difference between existing RenderTarget2D and new RenderTarget2D\nExisting RenderTarget2D: " + instance.renderTarget2Ds[name] + "\nNew RenderTarget2D" + target);
                }
            }
            else
            {
                instance.renderTarget2Ds.Add(name, target);
                RenderTargetBatch batch = new RenderTargetBatch(name, target);
                RenderManager.AddRenderTargetBatch(batch);
            }

            if (parent != null)
            {
                parent.assets.assets["renderTargets"].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            return instance.renderTarget2Ds[name];
        }

        public static void UnLoadScene(Scene newScene)
        {
            SceneAssetsPackage difference = SceneManager.activeScene.assets.Difference(newScene.assets);

            // By finding the difference between the current scene and the newScene I am given a list of all the assets only found in the current scene,
            // which must all be unloaded, as they will no longer be used

            foreach (string str in difference.gameObjects)
            {
                instance.gameObjects.Remove(str);
            }
            foreach (string str in difference.fonts)
            {
                instance.fonts.Remove(str);
            }
            foreach (string str in difference.models)
            {
                instance.models.Remove(str);
            }
            foreach (string str in difference.texture2Ds)
            {
                instance.texture2Ds.Remove(str);
            }
            foreach (string str in difference.renderTarget2Ds)
            {
                instance.renderTarget2Ds.Remove(str);
            }
            foreach (string str in difference.uiWidgets)
            {
                instance.uiWidgets.Remove(str);
            }
        }

        public static UIObject LoadUIObject(string name, Scene parent)
        {
            // TODO: UIWidget load logic

            if (parent != null)
            {
                parent.assets.uiWidgets.Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            return null;
        }
    }
}