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

        public List<RenderTargetDrawCallback> callbacks;

        public SpriteBatch spriteBatch;
        
        private Dictionary<string, RenderTargetBatch> renderTargetBatches;

        private bool isInit;
        private static RenderManager instance;
        public static RenderManager Instance(Microsoft.Xna.Framework.Game game, SpriteBatch spiteBatch)
        {
            instance = (instance == null) ? new RenderManager(game) : instance;

            return instance;
        }

        public RenderManager(Microsoft.Xna.Framework.Game game) : base(game)
        {
            callbacks = new List<RenderTargetDrawCallback>();
            renderTargetBatches = new Dictionary<string, RenderTargetBatch>();
        }
        public override void Initialize()
        {
            if (isInit)
                return;

            Resources.LoadRenderTarget2D("default", SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, true, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);

            Resources.LoadRenderTarget2D("UI", SceneManager.activeScene, GraphicsHelper.screen.Width, GraphicsHelper.screen.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            isInit = true;
        }

        public static void RegisterDrawCallback(string name, RenderTargetBatch.DrawCallback callback)
        {
            if (!instance.isInit)
            {
                instance.Initialize();
            }
            instance.renderTargetBatches[name].RegisterDrawCallBack(callback);
        }
        public static void UnRegisterDrawCallback(string name, RenderTargetBatch.DrawCallback callback)
        {
            instance.renderTargetBatches[name].UnRegisterDrawCallBack(callback);
        }
        
        public static void RegisterDrawCallback(RenderTargetDrawCallback callback, RenderTargetRenderer renderTargetRenderer)
        {
            instance.callbacks.Add(callback);
        }
        public static void UnRegisterDrawCallback(RenderTargetDrawCallback callback, RenderTargetRenderer renderTargetRenderer)
        {
            instance.callbacks.Remove(callback);
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
                batch.Value.spriteBatch = spriteBatch;
                batch.Value.Draw();
            }
            foreach (RenderTargetDrawCallback draw in callbacks)
            {
                draw();
            }
        }

        //public static void AddToRenderQueue()
        //{
        //    foreach (GameObject obj in gameObjectRenderTargets)
        //    {
        //        gameObjectRenderTargets.Add(obj);
        //    }
        //    foreach (UIVisual obj in UIRenderTargets)
        //    {
        //        UIRenderTargets.Add(obj);
        //    }
            
        //}
        //public void Render(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Begin();

        //    foreach (GameObject obj in gameObjectRenderTargets)
        //    {
        //        //spriteBatch.Draw()
        //    }
        //    foreach (UIVisual obj in UIRenderTargets)
        //    {
        //        //                                            focus stuff not implimented yet
        //        spriteBatch.Draw(obj.Sprite, obj.Bounds, null, obj.Colour[0] * obj.Opacity[0], obj.Rotation, obj.Origin, SpriteEffects.None, obj.Depth);
        //    }

        //    spriteBatch.End();
        //}
    }
}
