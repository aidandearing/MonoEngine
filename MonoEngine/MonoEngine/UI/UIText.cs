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
    public class UIText : UIObject
    {
        public TextRenderer textRenderer;
        public string Font
        {
            get
            {
                return textRenderer.Font.fontName;
            }
            set
            {
                textRenderer.Font = Resources.LoadAsset(new Font().GetType(), value, SceneManager.activeScene) as Font;
            }
        }
        
        public UIText(string name, UIAlignment boundsAlign, UIAlignment alignment, flags flag, string fontName, string text, int fontSize) : base(name, boundsAlign, alignment, flag)
        {
            textRenderer = TextRenderer.MakeTextRenderer(name, fontName);
            textRenderer.Size = fontSize;
            textRenderer.Text = text;
            textRenderer.Font = Resources.LoadAsset(new Font().GetType(), fontName, SceneManager.activeScene) as Font;

            //get the width and height of the string being drawn
            bounds.Width = (int)textRenderer.Font.GetFont(fontSize).MeasureString(text).X;
            bounds.Height = (int)textRenderer.Font.GetFont(fontSize).MeasureString(text).Y;

            Vector2 boundsAlignment = boundsAlign.GetAlignment(this, parent);
            bounds.X = (int)boundsAlignment.X;
            bounds.Y = (int)boundsAlignment.Y;

            textRenderer.Position = new Vector2(bounds.X, bounds.Y);
        }
    }
}
