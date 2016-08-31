using System.Collections.Generic;
using System.IO;
using MonoEngine.Render;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Game;
using MonoEngine.UI;

namespace MonoEngine.Assets
{
    public class Resources
    {
        private static Resources instance;
        private Resources()
        {
            models = new Dictionary<string, Model>();
            fonts = new Dictionary<string, Font>();
            texture2Ds = new Dictionary<string, Texture2D>();
            renderTarget2Ds = new Dictionary<string, RenderTarget2D>();
        }

        public static Resources Initialise()
        {
            instance = (instance == null) ? new Resources() : instance;
            return instance;
        }
        
        // There want to be as many lists with the same name here as there are in SceneAssetsPackage
        private Dictionary<string, GameObject> gameObjects;
        private Dictionary<string, Font> fonts;
        private Dictionary<string, Model> models;
        private Dictionary<string, Texture2D> texture2Ds;
        private Dictionary<string, RenderTarget2D> renderTarget2Ds;
        private Dictionary<string, UIObject> uiWidgets;

        public static GameObject LoadGameObject(string name, Scene parent)
        {
            return null;
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
                parent.assets.fonts.Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            return new Font(sizes.ToArray(), fonts.ToArray());
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
                parent.assets.models.Add(name);
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
                parent.assets.texture2Ds.Add(name);
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
                    throw new RenderExceptions.AssetAlreadyExists("Cannot resolve difference between existing RenderTarget2D and new RenderTarget2D\nExisting RenderTarget2D: " + instance.renderTarget2Ds[name] + "\nNew RenderTarget2D" + target);
                }
            }
            else
            {
                instance.renderTarget2Ds.Add(name, target);
            }

            if (parent != null)
            {
                parent.assets.renderTarget2Ds.Add(name);
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

            foreach(string str in difference.gameObjects)
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