using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Render;

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
                            this.BoundsAlign = align;
                            this.Bounds = ReaderToBounds(this, reader);
                            break;
                        case "behaviours":
                            this.Behaviours = ReaderToBehaviours(this, reader);
                            break;
                        case "objects":
                            this.ComponentRefs = ReaderToObjectRefs(this, reader);
                            break;
                        case "colour":
                            this.Colour = ReaderToColour(this, reader);
                            break;
                        case "opacity":
                            this.Opacity = ReaderToOpacity(this, reader);
                            break;
                        case "ref":
                            if (reader.Read() && reader["type"] == "object")
                                Ref = reader.Value;
                            break;
                    }
                }
            }
            Sprite = SpriteRenderer.MakeSpriteRenderer(Ref).sprite;
            SetOrigin(BoundsAlign);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            spriteBatch.Draw(Sprite, Bounds, null, Colour[i] * Opacity[i], Rotation, Origin, SpriteEffects.None, Depth);
            base.Draw(spriteBatch);
        }
        
    }
}
