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
            // Needs to try to get the model at that name in the models path & load it
            Texture2D texture = ContentHelper.Content.Load<Texture2D>(path + "/" + name);
            // add the model into the dictionary
            return new Sprite(texture, texture.Bounds, texture.Bounds, Color.White, 0.0f, Vector2.One, SpriteEffects.None);
            }
    }
}
