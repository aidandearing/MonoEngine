using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Assets
{
    public class RenderTarget2DWrapper
    {
        private RenderTarget2D renderTarget;

        /// <summary>
        /// NEVER USE THIS, IT IS USED SPECIFICALLY TO GET THE RUNTIME TYPE OF AN INSTANCE OF FONT, AND NOTHING ELSE
        /// </summary>
        internal RenderTarget2DWrapper() { }

        public RenderTarget2DWrapper(RenderTarget2D renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        public static implicit operator RenderTarget2D(RenderTarget2DWrapper _renderTarget)
        {
            return _renderTarget.renderTarget;
        }
    }
}
