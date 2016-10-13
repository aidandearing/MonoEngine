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
        public static object Ref(XmlReader reader)
        {
            // This will be called whenever a reader has found a <ref> element and should do whatever is appropriate to make that <ref> into an object
            Type type = null;

            type = Type.GetType(reader["type"], true, true);

            if (reader["name"] != null)
                return LoadAsset(type, reader["name"], SceneManager.activeScene);
            else
                return null;
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