using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Render
{
    public class RenderTargetSettings
    {
        public SpriteSortMode mode;
        public BlendState blend;
        public SamplerState sampler;
        public DepthStencilState depth;
        public RasterizerState rasteriser;
        public Effect effect;

        public RenderTargetSettings()
        {
            mode = SpriteSortMode.Deferred;
        }
    }
}
