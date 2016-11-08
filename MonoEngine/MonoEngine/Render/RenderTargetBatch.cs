using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Assets;

namespace MonoEngine.Render
{
    public class RenderTargetBatch
    {
        public delegate void DrawCallback();

        private RenderTarget2DWrapper renderTarget;
        public RenderTarget2DWrapper RenderTarget
        {
            get { return renderTarget; }
            set { renderTarget = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public RenderTargetSettings settings;

        public List<DrawCallback> callbacks;

        public RenderTargetBatch(string name, RenderTarget2DWrapper renderTarget, RenderTargetSettings settings)
        {
            callbacks = new List<DrawCallback>();
            Name = name;
            this.renderTarget = renderTarget;

            if (settings == null)
                this.settings = new RenderTargetSettings();
            else
                this.settings = settings;
        }

        public void RegisterDrawCallBack(DrawCallback callback)
        {
            callbacks.Add(callback);
        }

        public void UnRegisterDrawCallBack(DrawCallback callback)
        {
            callbacks.Add(callback);
        }

        public void Draw()
        {
            //set the rendertarget
            GraphicsHelper.graphicsDevice.SetRenderTarget(renderTarget);
            //clear the screen
            GraphicsHelper.graphicsDevice.Clear(Color.White * 0);
            //open the spritebatch
            GraphicsHelper.spriteBatch.Begin(settings.mode, settings.blend, settings.sampler, settings.depth, settings.rasteriser, settings.effect);

            foreach (DrawCallback draw in callbacks)
            {
                draw();
            }

            //close the spritebatch
            GraphicsHelper.spriteBatch.End();
            //default the graphicsDevice to draw back to the screen again
            GraphicsHelper.graphicsDevice.SetRenderTarget(null);
        }
    }
}
