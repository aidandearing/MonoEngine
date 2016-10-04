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
        private Texture2D image;
        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        private Rectangle imageRect;
        public Rectangle ImageRect
        {
            get { return imageRect; }
            set { ImageRect = value; }
        }

        private Rectangle sourceRect;
        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private SpriteEffects spriteEffects;
        public SpriteEffects SpriteEffects
        {
            get { return spriteEffects; }
            set { spriteEffects = value; }
        }

        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// NEVER USE THIS, IT IS USED SPECIFICALLY TO GET THE RUNTIME TYPE OF AN INSTANCE OF FONT, AND NOTHING ELSE
        /// </summary>
        internal Sprite() { }

        public Sprite(Texture2D image, Rectangle imageRect, Rectangle sourceRect, Color color, float rotation, float scale, SpriteEffects spriteEffects)
        {
            this.image = image;
            this.imageRect = imageRect;
            this.sourceRect = sourceRect;
            this.color = color;
            this.rotation = rotation;
            this.scale = scale;
            this.spriteEffects = spriteEffects;
        }

        public Sprite(XmlReader reader)
        {

        }


    }
}
