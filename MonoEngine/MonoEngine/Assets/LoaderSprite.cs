using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoEngine.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Assets
{
    class LoaderSprite : ResourceManagerLoader
    {
        public LoaderSprite(Type type) : base(type) { }

        public override object LoadAsset(string path, string name, Scene parent)
        {
            // Needs to try to get the texture at that name in the textures path & load it
            Texture2D texture = ContentHelper.Content.Load<Texture2D>(path + "/" + name);

            if (parent != null)
            {
                //load the texture into the dictionary
                parent.assets.assets[type].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }
            Sprite sprite = new Sprite(texture, texture.Bounds, texture.Bounds, Color.White, 0.0f, Vector2.One, SpriteEffects.None);
            
            return sprite;
            // add the model into the dictionary
            return new Sprite(texture, texture.Bounds, texture.Bounds, Color.White, 0.0f, Vector2.One, SpriteEffects.None);
        }
    }
}
