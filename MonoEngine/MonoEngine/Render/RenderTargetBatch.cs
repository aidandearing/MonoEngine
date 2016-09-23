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
        public SpriteBatch spriteBatch;
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
            foreach (DrawCallback draw in callbacks)
            {
                draw();
            }
        }
    }
}
