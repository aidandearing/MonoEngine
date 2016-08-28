using System.Collections.Generic;
using System.IO;
using MonoEngine.Render;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

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

        private Dictionary<string, Model> models;
        private Dictionary<string, Font> fonts;
        private Dictionary<string, Texture2D> texture2Ds;
        private Dictionary<string, RenderTarget2D> renderTarget2Ds;

        public static Model LoadModel(string name)
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
            // Pass the model at that key in the dictionary
            return instance.models[name];
        }

        public static Font LoadFont(string name)
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
                int.TryParse(split[6], out size);

                if (split[6] != null)
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
                return new Font(sizes.ToArray(), fonts.ToArray());
            }
            return null;
        }

        public static Texture2D LoadTexture2D(string name)
        {
            // Load a texture from a texture at the Assets/Textures/ + name.xnb pathway

            // Return the texture, not null
            return null;
        }

        public static void LoadRenderTarget2D(string name, int width, int height, bool mipMap, SurfaceFormat surfaceFormat, DepthFormat depthFormat, int multiSampleCount, RenderTargetUsage usage)
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
        }
    }
}