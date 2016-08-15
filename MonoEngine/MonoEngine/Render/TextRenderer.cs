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
        private static Dictionary<string, string> textSnippets = new Dictionary<string, string>();
        private TextRenderer(string name, string text) : base(name)
        {
            RenderManager.RegisterDrawCallBack(this, new RenderManager.DrawCallBack(Draw));
        }
        public void Draw()
        {
            
        }
        public static TextRenderer MakeTextRenderer(string name)
        {
            string text = "";

            if (!textSnippets.ContainsKey(name))
            {
                
            }
            return new TextRenderer(name, text);
        }
    }
}
