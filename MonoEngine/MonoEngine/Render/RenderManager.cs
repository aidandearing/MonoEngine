using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Game;
using MonoEngine.UI;
using MonoEngine.Assets;

namespace MonoEngine.Render
{
    public class RenderManager : DrawableGameComponent
    {
        public delegate void RenderTargetDrawCallback();

        public SortedList<int,RenderTargetDrawCallback> callbacks;
        
        private Dictionary<string, RenderTargetBatch> renderTargetBatches;

        

        private bool isInit;
        private static RenderManager instance;
        public static RenderManager Instance(Microsoft.Xna.Framework.Game game)
        {
            instance = (instance == null) ? new RenderManager(game) : instance;

            return instance;
        }

        public RenderManager(Microsoft.Xna.Framework.Game game) : base(game)
        {
            callbacks = new SortedList<int,RenderTargetDrawCallback>();
            renderTargetBatches = new Dictionary<string, RenderTargetBatch>();
        }
        public override void Initialize()
        {
            if (isInit)
                return;

            Resources.LoadRenderTarget2D("default", SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, true, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);

            Resources.LoadRenderTarget2D("UI", SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            Resources.LoadRenderTarget2D("screen", SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, true, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);

            RenderTargetRenderer.MakeRenderTargetRenderer("default", 0);

            RenderTargetRenderer.MakeRenderTargetRenderer("UI", 1);

            isInit = true;
        }

        public static RenderTargetBatch RegisterDrawCallback(string name, RenderTargetBatch.DrawCallback callback)
        {
            if (!instance.isInit)
            {
                instance.Initialize();
            }
            instance.renderTargetBatches[name].RegisterDrawCallBack(callback);

            return instance.renderTargetBatches[name];
        }
        public static void UnRegisterDrawCallback(string name, RenderTargetBatch.DrawCallback callback)
        {
            instance.renderTargetBatches[name].UnRegisterDrawCallBack(callback);
        }
        
        public static void RegisterDrawCallback(RenderTargetDrawCallback callback, RenderTargetRenderer renderTargetRenderer)
        {
            instance.callbacks.Add(renderTargetRenderer.Priority,callback);
        }
        public static void UnRegisterDrawCallback(RenderTargetDrawCallback callback, RenderTargetRenderer renderTargetRenderer)
        {
            instance.callbacks.Remove(renderTargetRenderer.Priority);
        }

        public static RenderTargetBatch GetRenderTargetBatch(string name)
        {
            if (instance.renderTargetBatches.ContainsKey(name))
            {
                return instance.renderTargetBatches[name];
            }
            else
            {
                return null;
            }
        }
        //make a new rendertargetbatch
        public static void AddRenderTargetBatch(RenderTargetBatch renderTargetBatch)
        {
            //add it to the list of batches 
            instance.renderTargetBatches.Add(renderTargetBatch.Name, renderTargetBatch);
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (KeyValuePair<string, RenderTargetBatch> batch in renderTargetBatches)
            {
                batch.Value.Draw();
            }
            foreach (KeyValuePair<int, RenderTargetDrawCallback> draw in callbacks)
            {
                draw.Value(); 
            }
            //this is the final draw that guarantees that the "screen" draws to the screen
            GraphicsHelper.graphicsDevice.SetRenderTarget(null);
            GraphicsHelper.spriteBatch.Begin();
            GraphicsHelper.spriteBatch.Draw(Resources.GetRenderTarget2D("screen"), GraphicsHelper.screen, Color.White);
            GraphicsHelper.spriteBatch.End();
        }
    }
}
