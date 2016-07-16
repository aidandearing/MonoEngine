using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;

namespace MonoEngine.UI
{
    class UIVisual : UIObject
    {
        public string Ref { get; set; }
        public Color[] Colour { get; set; }
        public float[] Opacity { get; set; }

        public static Color[] ReaderToColour(UIVisual parent, XmlReader reader)
        {
            Color[] colours = new Color[2];

            int depth = reader.Depth;

            while (reader.Read() && reader.Depth > depth)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "default":
                            //colours[0] = new Value(reader.Value).ToColour();
                            break;
                        case "focus":
                            //colours[1] = new Value(reader.Value).ToColour();
                            break;
                    }
                }
            }
            return colours;
        }
        public static float[] ReaderToOpacity(UIObject parent, XmlReader reader)
        {
            float[] opacity = new float[2];

            int depth = reader.Depth;

            while (reader.Read() && reader.Depth > depth)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "default":
                            //opacity[0] = new Value(reader.Value).ToFloat();
                            break;
                        case "focus":
                            //opacity[1] = new Value(reader.Value).ToFloat();
                            break;
                          
                    }
                }
            }
            return opacity;
        }

    }
}
