using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoEngine.Game;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
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
            GraphicsHelper.spriteBatch.Draw(sprite.Texture, null, sprite.DestinationRect, sprite.SourceRect, sprite.Origin, sprite.Rotation, sprite.Scale, sprite.Colour * (sprite.Colour.A / 255.0f), sprite.SpriteEffect, 0.0f);
        }

        public static SpriteRenderer MakeSpriteRenderer(string name, string asset, string targetName = null)
        {
            return new SpriteRenderer(name, Resources.LoadAsset(new Sprite().GetType(), asset, SceneManager.activeScene) as Sprite, targetName);
        }
    }
}
