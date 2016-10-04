using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Render;
using MonoEngine.Assets;
using MonoEngine.Game;

namespace MonoEngine.UI
{
    class UIText : UIObject
    {
        //uint because font sizes can not be negative
        public uint fontSize { get; set; }

        public string text { get; set; }

        private Font font;

        public string Font
        {
            get { return font.fontName; }

            set { font = Resources.LoadAsset(new Font().GetType(), value, SceneManager.activeScene) as Font; }
        }
        
        public UIText(string name, Rectangle bounds, UIAlignment boundsAlign, UIAlignment alignment, flags flag, string fontName, string text, uint fontSize) : base(name, bounds, boundsAlign, alignment, flag)
        {
            this.fontSize = fontSize;
            this.text = text;
            font = Resources.LoadAsset(new Font().GetType(), fontName, SceneManager.activeScene) as Font;
        }

        
    }
}
