using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;

namespace MonoEngine.Assets
{
    public class Sprite
    {
        public Vector2 Origin { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle DestinationRect { get; set; }
        public Rectangle SourceRect { get; set; }
        public Color Colour { get; set; }
        public SpriteEffects SpriteEffect { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }

        public Sprite() { }

        public Sprite(Texture2D texture, Rectangle destinationRect, Rectangle sourceRect, Color colour, float rotation, Vector2 scale, SpriteEffects spriteEffects)
        {
            Texture = texture;
            DestinationRect = destinationRect;
            SourceRect = sourceRect;
            Colour = colour;
            Rotation = rotation;
            Scale = scale;
            SpriteEffect = spriteEffects;
            Origin = new Vector2(DestinationRect.Center.X, DestinationRect.Center.Y);
        }

        public static implicit operator Texture2D(Sprite _texture)
        {
            return _texture.Texture;
        }
    }
}
