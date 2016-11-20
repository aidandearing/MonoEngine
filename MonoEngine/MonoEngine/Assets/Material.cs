using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Assets
{
    public class Material
    {
        public Effect effect;

        public Material() { }

        public Material(Effect effect)
        {
            this.effect = effect;
        }

        public static implicit operator Effect(Material mat) { return mat.effect; }
    }
}
