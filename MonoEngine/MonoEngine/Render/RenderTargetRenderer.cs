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
        private RenderTarget2D renderable;
        private RenderTarget2D target;

        private int priority;
        public int Priority
        {
            get { return priority; }
            private set { priority = value; }
        }

        private static Dictionary<string, RenderTargetRenderer> renderTargetInstances;
        internal RenderTargetRenderer(int priority, string name, RenderTarget2D renderable, RenderTarget2D target = null) : base(name)
        {
            if (target == null)
            {
                target = Resources.GetRenderTarget2D("screen");
            }
            this.target = target;
            this.renderable = renderable;
            this.priority = priority;
            RenderManager.RegisterDrawCallback(new RenderManager.RenderTargetDrawCallback(Draw), this);

            if (renderTargetInstances == null)
            {
                renderTargetInstances = new Dictionary<string, RenderTargetRenderer>();
            }
        }
        public void Draw()
        {
            GraphicsHelper.graphicsDevice.SetRenderTarget(target);

            GraphicsHelper.spriteBatch.Begin();

            GraphicsHelper.spriteBatch.Draw(renderable, GraphicsHelper.screen, Microsoft.Xna.Framework.Color.White);
            
            GraphicsHelper.spriteBatch.End();
        }

        public static RenderTargetRenderer MakeRenderTargetRenderer(string name, int priority)
        {
            if (renderTargetInstances == null)
            {
                renderTargetInstances = new Dictionary<string, RenderTargetRenderer>();
            }
            //check if there is already an instance of this renderer
            if (renderTargetInstances.ContainsKey(name))
            {
                return renderTargetInstances[name];
            }
            else
            {
                //there is not a rendertargetrenderer so check to see if there is a rendertarget
                if (Resources.CheckForAsset(name, new RenderTarget2DWrapper().GetType()))
                {
                    //make a new rendertargetrenderer with the rendertarget found
                    return new RenderTargetRenderer(priority, name, Resources.GetRenderTarget2D(name));
                }
                else
                {
                    //no rendertarget found so make a new rendertargetrenderer with a new rendertarget with the name specified
                    return new RenderTargetRenderer(priority, name, Resources.LoadRenderTarget2D(name, SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents));
                }
            }        
        }
    }
}
