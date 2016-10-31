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
        public Vector2 origin { get; set; }
        public Texture2D texture { get; set; }
        public Rectangle destinationRect { get; set; }
        public Rectangle sourceRect { get; set; }
        public Color color { get; set; }
        public SpriteEffects spriteEffects { get; set; }
        public float rotation { get; set; }
        public Vector2 scale { get; set; }

        /// <summary>
        /// NEVER USE THIS, IT IS USED SPECIFICALLY TO GET THE RUNTIME TYPE OF AN INSTANCE OF FONT, AND NOTHING ELSE
        /// </summary>
        internal Sprite() { }

        public Sprite(Texture2D texture, Rectangle destinationRect, Rectangle sourceRect, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects)
        {
            this.texture = texture;
            this.destinationRect = destinationRect;
            this.sourceRect = sourceRect;
            this.color = color;
            this.rotation = rotation;
            this.scale = scale;
            this.spriteEffects = spriteEffects;
            origin = new Vector2(destinationRect.Center.X, destinationRect.Center.Y);
        }
        public static implicit operator Texture2D(Sprite _texture)
        {
            return _texture.texture;
        }
        public Sprite(XmlReader reader)
        {

        }


    }
}
