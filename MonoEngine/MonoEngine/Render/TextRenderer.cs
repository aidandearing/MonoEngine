using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Game;

namespace MonoEngine.Render
{
    public class TextRenderer : GameObject
    {
        //TODO: implement drawing to rendertargets that are used as textures for models
        //public Model model { get; set; }
        public Font font { get; set; }

        private TextRenderer(string name, string text) : base(name)
        {
            RenderManager.RegisterDrawCallBack(this, new RenderManager.DrawCallBack(Draw));
        }
        public void Draw()
        {
            
        }
    }
}
