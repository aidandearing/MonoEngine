using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoEngine.Game;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.UI;

namespace MonoEngine.Render
{
    public class SpriteRenderer : GameObject
    {
        private static Dictionary<string, Texture2D> sprites = new Dictionary<string, Texture2D>();

        public Texture2D sprite;

        private SpriteRenderer(string name, Texture2D sprite) : base(name)
        {
            this.sprite = sprite;
            RenderManager.RegisterDrawCallBack(this, new RenderManager.DrawCallBack(Draw));

        }
        public void Draw()
        {
            //RenderManager.spriteBatch.Draw(Sprite, Bounds, null, Colour[0] * Opacity[0], Rotation, Origin, SpriteEffects.None, Depth);
        }

        public static SpriteRenderer MakeSpriteRenderer(string name)
        {
            //if the dictionary doesnt contain the key 
            if (!sprites.ContainsKey(name))
            {
                //load the sprite 
                Texture2D sprite = ContentHelper.Content.Load<Texture2D>("Assets/Sprites/" + name);

                //add to the dictionary
                sprites.Add(name, sprite);
            }
            return new SpriteRenderer(name, sprites[name]);
        }

        
    }
}
