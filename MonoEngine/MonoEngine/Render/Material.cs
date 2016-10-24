using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Render
{
    class Material
    {
        private Effect effect;
        public Effect Effect
        {
            get
            {
                return effect;
            }
            set
            {
                effect = value;
            }
        }
    }
}
