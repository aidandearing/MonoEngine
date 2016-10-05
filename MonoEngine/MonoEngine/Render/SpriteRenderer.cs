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
            //batch.spriteBatch.Draw(sprite.Image, sprite.ImageRect, sprite.SourceRect, sprite.Color, sprite.Rotation, sprite.SourceRect.Center, sprite.SpriteEffects, 0.0f);
            //RenderManager.spriteBatch.Draw(Sprite, Bounds, null, Colour[0] * Opacity[0], Rotation, Origin, SpriteEffects.None, Depth);
        }

        public static SpriteRenderer MakeSpriteRenderer(string name)
        {
            return new SpriteRenderer(name, Resources.LoadAsset(new Sprite().GetType(), name, SceneManager.activeScene) as Sprite);
        }
    }
}
