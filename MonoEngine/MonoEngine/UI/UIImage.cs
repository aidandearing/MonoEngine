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
    class UIImage : UIVisual
    {
        public UIImage(XmlReader reader)
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
                    }
                }
            }
            SetOrigin(BoundsAlign);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Texture2D texture
            
            base.Draw(spriteBatch);
        }
    }
}
