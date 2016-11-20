using Microsoft.Xna.Framework;

namespace MonoEngine
{
    public class ColourHelper
    {
        public static Color HSVtoRGB(float h, float s, float v)
        {
            h = (h + 180.0f * Time.DeltaTime) % 360.0f;

            float r = 0;
            float g = 0;
            float b = 0;

            int i;
            float f, p, q, t;
            if (s == 0)
            {
                // achromatic (grey)
                r = g = b = v;
                return new Color(r, g, b);
            }
            // sector 0 to 5
            float h_t = h / 60;
            i = (int)System.Math.Floor(h_t);
            // factorial part of h
            f = h_t - i;
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            switch (i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }

            return new Color(r, g, b);
        }

        public static Color HSVtoRGB(float h, float s, float v, float a)
        {
            h = (h + 180.0f * Time.DeltaTime) % 360.0f;

            float r = 0;
            float g = 0;
            float b = 0;

            int i;
            float f, p, q, t;
            if (s == 0)
            {
                // achromatic (grey)
                r = g = b = v;
                return new Color(r, g, b, a);
            }
            // sector 0 to 5
            float h_t = h / 60;
            i = (int)System.Math.Floor(h_t);
            // factorial part of h
            f = h_t - i;
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            switch (i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }

            return new Color(r, g, b, a);
        }
    }
}
