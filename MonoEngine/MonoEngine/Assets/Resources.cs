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
                    return instance.LoadCamera(reader);
                case TypeOfRef.GameObject:
                    return instance.LoadGameObject(reader);
                case TypeOfRef.Model:
                    return instance.LoadModel(reader);
                case TypeOfRef.ModelRenderer:
                    return instance.LoadModelRenderer(reader);
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

            handler_GameObject = new ResourceManager<GameObject>("Assets/Objects");
            handler_Model = new ResourceManager<Model>("Assets/Models");
            handler_Font = new ResourceManager<Font>("Assets/Fonts");
            handler_Sprite = new ResourceManager<Sprite>("Assets/Textures");
            handler_RenderTarget2D = new ResourceManager<RenderTarget2D>("Assets/Shaders");
            handler_UIObject = new ResourceManager<UIObject>("Assets/UI");

            resourceManagers.Add(typeof(GameObject), handler_GameObject);
            resourceManagers.Add(typeof(Model), handler_Model);
            resourceManagers.Add(typeof(Font), handler_Font);
            resourceManagers.Add(typeof(Sprite), handler_Sprite);
            resourceManagers.Add(typeof(RenderTarget2D), handler_RenderTarget2D);
            resourceManagers.Add(typeof(UIObject), handler_UIObject);

            

        }

        public static Resources Initialise()
        {
            instance = (instance == null) ? new Resources() : instance;
            return instance;
        }

        // The strange dictionary of all resource managers, keyed by Type
        private Dictionary<Type, ResourceMetaData> resourceManagers;

        // Quick access handlers to each ResourceManager supported by default
        private ResourceManager<GameObject> handler_GameObject;
        private ResourceManager<Model> handler_Model;
        private ResourceManager<Font> handler_Font;
        private ResourceManager<Sprite> handler_Sprite;
        private ResourceManager<RenderTarget2D> handler_RenderTarget2D;
        private ResourceManager<UIObject> handler_UIObject;

        public static bool CheckForAsset<Type>(string name)
        {
            return ((ResourceManager<Type>)instance.resourceManagers[typeof(Type)]).ContainsResource(name);
        }

        public static void RemoveAsset<Type>(string name)
        {
            (instance.resourceManagers[typeof(Type)] as ResourceManager<Type>).RemoveResource(name);
        }

        public static RenderTarget2D GetRenderTarget2D(string name)
        {
            if (instance.resourceManagers[typeof(RenderTarget2D)].ContainsResource(name))
            {
                return instance.handler_RenderTarget2D.GetResource(name);
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
            if (!instance.handler_GameObject.ContainsResource(name))
            {
                // Needs to try to get the model at that name in the models path & load it
                GameObject asset = null;
                using (XmlReader reader = XmlReader.Create(@"./Content/" + instance.handler_GameObject.Path + name + ".xml"))
                {
                    asset = instance.LoadGameObject(reader);
                }
                // add the model into the dictionary
                instance.handler_GameObject.AddResource(name, asset);
                return asset;
            }

            if (parent != null)
            {
                parent.assets.AddAsset(name, typeof(GameObject));
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            // Pass the texture at that key in the dictionary
            return instance.handler_GameObject.GetResource(name);
        }

        public static Font LoadFont(string name, Scene parent)
        {
            // Load a font from a name at the Assets/Fonts/ + name#.xnb pathway (plus all it's sizes)
            if (!instance.handler_Font.ContainsResource(name))
            {
                string[] paths = Directory.GetFiles(@"./Content/" + instance.handler_Font.Path, name + "_*.xnb");

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
                    parent.assets.assets[typeof(Font)].Add(name);
                }
                else
                {
                    // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
                }

                Font font = new Font(sizes.ToArray(), fonts.ToArray(), name);
                instance.handler_Font.AddResource(name, font);
                return font;
                
            }

            return instance.handler_Font.GetResource(name);
        }

        public static Model LoadModel(string name, Scene parent)
        {
            // Load a model from a name at the Assets/Models/ + name.xnb pathway

            // If the model isn't already loaded (it's key isn't found in the dictionary)
            if (!instance.handler_Model.ContainsResource(name))
            {
                // Needs to try to get the model at that name in the models path & load it
                Model model = ContentHelper.Content.Load<Model>("Assets/Models/" + name);
                // add the model into the dictionary
                instance.handler_Model.AddResource(name, model);
                return model;
            }

            if (parent != null)
            {
                parent.assets.assets[typeof(Model)].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            // Pass the model at that key in the dictionary
            return instance.handler_Model.GetResource(name);
        }

        public static Sprite LoadTexture2D(string name, Scene parent)
        {
            // Load a texture from a texture at the Assets/Textures/ + name.xnb pathway

            // If the model isn't already loaded (it's key isn't found in the dictionary)
            if (!instance.handler_Sprite.ContainsResource(name))
            {
                // Needs to try to get the model at that name in the models path & load it
                Texture2D texture = ContentHelper.Content.Load<Texture2D>("Assets/Textures/" + name);
                // add the model into the dictionary
                Sprite sprite = new Sprite(texture, texture.Bounds, texture.Bounds, Color.White, 0.0f, 1.0f, SpriteEffects.None);
                instance.handler_Sprite.AddResource(name, sprite);
                return sprite;
            }

            if (parent != null)
            {
                parent.assets.assets[typeof(Texture2D)].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            // Pass the texture at that key in the dictionary
            return instance.handler_Sprite.GetResource(name);
        }

        public static RenderTarget2D LoadRenderTarget2D(string name, Scene parent, int width, int height, bool mipMap, SurfaceFormat surfaceFormat, DepthFormat depthFormat, int multiSampleCount, RenderTargetUsage usage)
        {
            RenderTarget2D target = new RenderTarget2D(GraphicsHelper.graphicsDevice, width, height, mipMap, surfaceFormat, depthFormat, multiSampleCount, usage);

            if (instance.handler_RenderTarget2D.ContainsResource(name))
            {
                if (target != instance.handler_RenderTarget2D.GetResource(name))
                {
                    throw new AssetExceptions.AssetAlreadyExists("Cannot resolve difference between existing RenderTarget2D and new RenderTarget2D\nExisting RenderTarget2D: " + instance.handler_RenderTarget2D.GetResource(name) + "\nNew RenderTarget2D" + target);
                }
            }
            else
            {
                instance.handler_RenderTarget2D.AddResource(name, target);
                RenderTargetBatch batch = new RenderTargetBatch(name, target);
                RenderManager.AddRenderTargetBatch(batch);
                return target;
            }

            if (parent != null)
            {
                parent.assets.assets[typeof(RenderTarget2D)].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            return instance.handler_RenderTarget2D.GetResource(name);
        }

        public static UIObject LoadUIObject(string name, Scene parent)
        {
            // TODO: UIWidget load logic

            if (parent != null)
            {
                parent.assets.assets[typeof(UIObject)].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            return null;
        }

        public static void UnloadScene(Scene newScene)
        {
            SceneAssetsPackage difference = SceneManager.activeScene.assets.Difference(newScene.assets);

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

        public static void AddResourceManager<T>(string path)
        {
            if (!instance.resourceManagers.ContainsKey(typeof(T)))
            {
                instance.resourceManagers.Add(typeof(T), new ResourceManager<T>(path));
            }
            else
            {
                throw new AssetExceptions.ResourceManagerOfTypeAlreadyExists("Cannot create ResourceManager of type " + typeof(T) + " with path " + path + " as a ResourceManager of that type already exists");
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