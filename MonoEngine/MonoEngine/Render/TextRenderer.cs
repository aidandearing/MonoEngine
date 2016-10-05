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
        public Font font { get; set; }

        private RenderTargetBatch batch;

        private TextRenderer(string name, string text, string targetName = null) : base(name)
        {
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
            //batch.spriteBatch.DrawString()
        }
    }
}
