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
        public class Settings
        {
            public SpriteSortMode mode = SpriteSortMode.Deferred;
            public BlendState blend = BlendState.AlphaBlend;
            public SamplerState sampler = SamplerState.LinearWrap;
            public DepthStencilState depth = DepthStencilState.Default;
            public RasterizerState rasteriser = RasterizerState.CullNone;
            public Effect effect = null;// new BasicEffect(GraphicsHelper.graphicsDevice);

            public Settings() { }
        }

        private static Dictionary<string, RenderTargetRenderer> renderTargetInstances;

        public Settings settings;

        private RenderTarget2D renderable;
        private RenderTarget2D target;

        private int priority;
        public int Priority
        {
            get { return priority; }
            private set { priority = value; }
        }

        internal RenderTargetRenderer(int priority, string name, Settings settings, RenderTarget2D renderable, RenderTarget2D target = null) : base(name)
        {
            if (target == null)
            {
                target = Resources.GetRenderTarget2D("screen");
            }

            this.target = target;
            this.renderable = renderable;
            this.priority = priority;
            this.settings = settings;

            RenderManager.RegisterDrawCallback(new RenderManager.RenderTargetDrawCallback(Draw), this);

            if (renderTargetInstances == null)
            {
                renderTargetInstances = new Dictionary<string, RenderTargetRenderer>();
            }

            renderTargetInstances.Add(name, this);
        }

        public void Draw()
        {
            GraphicsHelper.graphicsDevice.SetRenderTarget(target);

            GraphicsHelper.spriteBatch.Begin(settings.mode, settings.blend, settings.sampler, settings.depth, settings.rasteriser, settings.effect);

            GraphicsHelper.spriteBatch.Draw(renderable, GraphicsHelper.screen, Microsoft.Xna.Framework.Color.White);
            
            GraphicsHelper.spriteBatch.End();
        }

        public static RenderTargetRenderer MakeRenderTargetRenderer(string name, Settings settings, int priority)
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
                    return new RenderTargetRenderer(priority, name, settings, Resources.GetRenderTarget2D(name));
                }
                else
                {
                    //no rendertarget found so make a new rendertargetrenderer with a new rendertarget with the name specified
                    return new RenderTargetRenderer(priority, name, settings, Resources.LoadRenderTarget2D(name, SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents));
                }
            }        
        }
    }
}
