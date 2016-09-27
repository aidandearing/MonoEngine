using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Render
{
    public class RenderTargetBatch
    {
        public delegate void DrawCallback();
        private RenderTarget2D renderTarget;
        public RenderTarget2D RenderTarget
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

        public List<DrawCallback> callbacks;

        public RenderTargetBatch(string name, RenderTarget2D renderTarget)
        {
            callbacks = new List<DrawCallback>();
            Name = name;
            this.renderTarget = renderTarget;
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
            GraphicsHelper.spriteBatch.Begin();

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
