using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoEngine.Game;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Render;
using MonoEngine.Assets;

namespace MonoEngine.Render
{
    public class RenderTargetRenderer : GameObject
    {
        private RenderTarget2D renderTarget;

        private static Dictionary<string, RenderTargetRenderer> renderTargetInstances;
        internal RenderTargetRenderer(string name, RenderTarget2D renderable) : base(name)
        {
            renderTarget = renderable;
            RenderManager.RegisterDrawCallback(new RenderManager.RenderTargetDrawCallback(Draw), this);

            if (renderTargetInstances == null)
            {
                renderTargetInstances = new Dictionary<string, RenderTargetRenderer>();
            }
        }
        public void Draw()
        {
            //RenderManager.spriteBatch.Draw(renderTarget, renderTarget.Bounds, Microsoft.Xna.Framework.Color.White);
        }

        public static RenderTargetRenderer MakeRenderTargetRenderer(string name)
        {
            //check if there is already an instance of this renderer
            if (renderTargetInstances.ContainsKey(name))
            {
                return renderTargetInstances[name];
            }
            else
            {
                //there is not a rendertargetrenderer so check to see if there is a rendertarget
                if (Resources.CheckForAsset<RenderTarget2D>(name))
                {
                    //make a new rendertargetrenderer with the rendertarget found
                    return new RenderTargetRenderer(name, Resources.GetRenderTarget2D(name));
                }
                else
                {
                    //no rendertarget found so make a new rendertargetrenderer with a new rendertarget with the name specified
                    return new RenderTargetRenderer(name, Resources.LoadRenderTarget2D(name, SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents));
                }
            }        
        }
    }
}
