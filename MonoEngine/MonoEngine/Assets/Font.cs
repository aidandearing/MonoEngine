using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Render
{
    public class Font
    {
        private static Dictionary<string, Font> fonts = new Dictionary<string, Font>();
        public int[] fontSizes;
        public SpriteFont[] fontStyles;
        public string fontName;

        public Font(int[] fontSizes, SpriteFont[] fontStyles, string fontName)
        {
            this.fontSizes = fontSizes;
            this.fontStyles = fontStyles;
            this.fontName = fontName;
            fonts = new Dictionary<string, Font>();
            
        }

        public SpriteFont GetFont(int size)
        {
            for (int i = 0; i < fontSizes.Length; i++)
            {
                if (size <= fontSizes[i] || i == fontSizes.Length - 1)
                {
                    return fontStyles[i];
                }
            }
            return null;
        }
    }
}
