using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Game;
using MonoEngine.Assets;

namespace MonoEngine.Render
{
    public class TextRenderer : GameObject
    {
        //TODO: implement drawing to rendertargets that are used as textures for models
        //public Model model { get; set; }
        public Font Font { get; set; }
        public string Text { get; set; }

        private int size;
        public int Size
        {
            get { return size; }
            set { size = value; font = Font.GetFont(size); }
        }

        public Vector2 Position { get; set; }
        public Color Colour { get; set; }

        private SpriteFont font;
        private RenderTargetBatch batch;

        private TextRenderer(string name, Font font, string text, int size, string targetName = null) : base(name)
        {
            Text = text;
            Font = font;
            Size = size;
            Colour = Color.White;
            Position = Vector2.Zero;

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
            GraphicsHelper.spriteBatch.DrawString(Font.GetFont(Size), Text, Position, Colour, 0, Vector2.Zero, (float)Size/(float)Font.GetSize(Size), SpriteEffects.None, 0);
        }

        public static TextRenderer MakeTextRenderer(string name, string asset, string targetName = null)
        {
            return new TextRenderer(name, Resources.LoadAsset(new Font().GetType(), asset, SceneManager.activeScene) as Font, "text", 12);
        }
    }
}
