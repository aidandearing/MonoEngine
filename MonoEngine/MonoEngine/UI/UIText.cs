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
        
        public UIText(string name, Rectangle bounds, UIAlignment boundsAlign, UIAlignment alignment, flags flag, string fontName, string text, int fontSize) : base(name, bounds, boundsAlign, alignment, flag)
        {
            textRenderer = TextRenderer.MakeTextRenderer(name, fontName);
            textRenderer.Size = fontSize;
            textRenderer.Text = text;
            textRenderer.Font = Resources.LoadAsset(new Font().GetType(), fontName, SceneManager.activeScene) as Font;
            textRenderer.Position = origin;
        }
    }
}
