using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Render
{
    class Material
    {
        public Effect Effect { get; set; }
        public Texture2D Diffuse { get; set; }
        public Texture2D Specular { get; set; }
        public Texture2D Normal { get; set; }

        /// <summary>
        /// NEVER USE THIS, IT IS USED SPECIFICALLY TO GET THE RUNTIME TYPE OF AN INSTANCE OF FONT, AND NOTHING ELSE
        /// </summary>
        internal Material() { }

        public Material(Effect effect, Texture2D diffuse, Texture2D specular, Texture2D normal)
        {
            Effect = effect;
            Diffuse = diffuse;
            Specular = specular;
            Normal = normal;
        }
    }
}
