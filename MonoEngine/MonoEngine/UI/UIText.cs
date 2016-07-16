using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.UI
{
    class UIText : UIVisual
    {
        public int Size { get; set; }
        public string Text { get; set; }
        public string TextToDraw { get; set; }

        public UIText(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "bounds":
                            Alignment align = new Alignment();
                            Enum.TryParse<Alignment>(reader["origin"], out align);
                            BoundsAlign = align;
                            Bounds = ReaderToBounds(this, reader);
                            break;
                        case "behaviours":
                            Behaviours = ReaderToBehaviours(this, reader);
                            break;
                        case "objects":
                            ComponentRefs = ReaderToObjectRefs(this, reader);
                            break;
                        case "colour":
                            Colour = ReaderToColour(this, reader);
                            break;
                        case "opacity":
                            Opacity = ReaderToOpacity(this, reader);
                            break;
                        case "ref":
                            if (reader.Read() && reader["type"] == "object")
                                Ref = reader.Value;
                            break;
                        case "size":
                            Size = int.Parse(reader.Value);
                            break;
                        case "text":
                            Text = reader.Value;
                            break;
                    }
                }
            }
            TextToDraw = CalculateText();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString()
            base.Draw(spriteBatch);
        }
        public string CalculateText()
        {
            string text = Text;

            return Text;
        }
    }
}
