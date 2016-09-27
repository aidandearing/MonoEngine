using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoEngine.Game;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.UI;
using MonoEngine.Assets;

namespace MonoEngine.Render
{
    public class SpriteRenderer : GameObject
    {
        //private static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        public Sprite sprite;

        private RenderTargetBatch batch;

        private SpriteRenderer(string name, Sprite sprite, string targetName = null) : base(name)
        {
            this.sprite = sprite;

            if (targetName == null)
            {
                batch = RenderManager.RegisterDrawCallback("UI", new RenderTargetBatch.DrawCallback(Draw));
            }
            else
            {
                batch = RenderManager.RegisterDrawCallback(targetName, new RenderTargetBatch.DrawCallback(Draw));
            }

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
