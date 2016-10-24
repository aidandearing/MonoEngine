using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Assets
{
    class EffectWrapper
    {
        private Microsoft.Xna.Framework.Graphics.Effect effect;

        /// <summary>
        /// NEVER USE THIS, IT IS USED SPECIFICALLY TO GET THE RUNTIME TYPE OF AN INSTANCE OF FONT, AND NOTHING ELSE
        /// </summary>
        internal EffectWrapper() { }

        public EffectWrapper(Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            this.effect = effect;
        }

        public static implicit operator Microsoft.Xna.Framework.Graphics.Effect(EffectWrapper _effect)
        {
            return _effect.effect;
        }
    }
}
