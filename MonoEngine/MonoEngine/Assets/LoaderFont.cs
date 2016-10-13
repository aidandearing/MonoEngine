using System;
using System.IO;
using System.Collections.Generic;
using MonoEngine.Game;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Assets
{
    class LoaderFont : ResourceManagerLoader
    {
        public LoaderFont(Type type) : base(type) { }

        public override object LoadAsset(string path, string name, Scene parent)
        {
            string[] paths = Directory.GetFiles(@"./Content/" + path, name + "_*.spritefont");

            char[] delimiters = { '.', '\\', '_' };

            List<int> sizes = null;
            int size = 0;
            List<SpriteFont> fonts = new List<SpriteFont>();
            foreach (string pathSub in paths)
            {
                string[] split = pathSub.Split(delimiters);
                // path looks like this .\\Content\\Assets\\Fonts\\name_*.xnb
                // delimited it becomes split[0] = "" | split[1] = "" | split[2] = "Content" | split[3] = "Assets" | split[4] = "Fonts" | split[5] = "name" | split[6] = "*" | split[7] = "xnb"

                if (split[3] != null && int.TryParse(split[3], out size))
                {
                    if (sizes == null)
                    {
                        sizes = new List<int>();
                        sizes.Add(size);
                        fonts.Add(ContentHelper.Content.Load<SpriteFont>(path + "/" + name + "_" + split[3]));
                    }
                    else
                    {
                        sizes.Add(size);
                        fonts.Add(ContentHelper.Content.Load<SpriteFont>(path + "/" + name + "_" + split[3]));
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

            return new Font(sizes.ToArray(), fonts.ToArray(), name);
        }
    }
}
