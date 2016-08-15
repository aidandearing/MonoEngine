using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Render
{
    class Font
    {
        public SpriteFont[] fontsize;
        public string fontName;
        private static Dictionary<string, Font> fonts = new Dictionary<string, Font>();

        public Font(SpriteFont fontSize8, SpriteFont fontSize16, SpriteFont fontSize32, SpriteFont fontSize64, SpriteFont fontSize128, SpriteFont fontSize256)
        {
            fontsize = new SpriteFont[6];
            fontsize[0] = fontSize8;
            fontsize[1] = fontSize16;
            fontsize[2] = fontSize32;
            fontsize[3] = fontSize64;
            fontsize[4] = fontSize128;
            fontsize[5] = fontSize256;

            fonts = new Dictionary<string, Font>();
        }
        public static Font MakeFont(string name)
        {
            SpriteFont[] spriteFont = new SpriteFont[6];

            if (!fonts.ContainsKey(name))
            {
                for (int fs = 4; fs <= 256; fs = fs * 2)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        spriteFont[i] = GetFont(name, fs);
                    }   
                }
            }
            Font f = new Font(spriteFont[0], spriteFont[1], spriteFont[2], spriteFont[3], spriteFont[4], spriteFont[5]);
            fonts.Add(name, f);
            return f;
        }
        public static SpriteFont GetFont(string name, int value)
        {
            if (fonts.ContainsKey(name))
            {
                if (value < 8)
                    value = 8;
                if (value > 8 && value < 16)
                    value = 16;
                if (value > 16 && value < 32)
                    value = 32;
                if (value > 32 && value < 64)
                    value = 64;
                if (value > 64 && value < 128)
                    value = 128;
                if (value > 128 && value < 256)
                    value = 256;
                if (value > 256)
                    value = 256;

                SpriteFont spriteFont = ContentHelper.Content.Load<SpriteFont>("Assets/Fonts/" + name + value.ToString());

                return spriteFont;
            }
            return null;
        }
    }
}
