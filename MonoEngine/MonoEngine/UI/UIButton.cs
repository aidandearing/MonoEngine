using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Render;

namespace MonoEngine.UI
{
    class UIButton : UIVisual
    {
        private bool IsHovering { get; set; }
        public UIText ButtonText { get; set; }

        public UIButton(XmlReader reader)
        {
            ButtonText = new UIText(reader);


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
                            //make sure the text is centered on the button
                            ButtonText.Origin = this.Origin;
                            this.Bounds = ReaderToBounds(this, reader);
                            break;
                        case "behaviours":
                            this.Behaviours = ReaderToBehaviours(this, reader);
                            break;
                        case "objects":
                            this.ComponentRefs = ReaderToObjectRefs(this, reader);
                            break;
                        case "opacity":
                            this.Opacity = ReaderToOpacity(this, reader);
                            break;
                        case "colour":
                            this.Colour = ReaderToColour(this, reader);
                            break;
                        case "ref":
                            if (reader["type"] == "object" && reader.Read())
                                Ref = reader.Value;
                            break;
                    }
                }
            }
            Sprite = SpriteRenderer.MakeSpriteRenderer(Ref).sprite;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            
            spriteBatch.Draw(Sprite, Bounds, null, Colour[i] * Opacity[i], Rotation, Origin, SpriteEffects.None, Depth);
            ButtonText.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void Update()
        {
            MouseState state = Mouse.GetState();
            Point mousePos = new Point(state.X, state.Y);
            Rectangle ButtonRect = new Rectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
            if (ButtonRect.Contains(mousePos))
            {
                IsHovering = true;

                OnMouseHover();

                if (state.LeftButton == ButtonState.Pressed)
                {
                    OnMouseClick();
                }
            }
            else
            {
                IsHovering = false;
            }

            base.Update();
        }
        public void OnMouseClick()
        {
        }
        public void OnMouseHover()
        {
        }
    }
}
