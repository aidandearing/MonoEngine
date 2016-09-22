using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Game;
using MonoEngine.UI;

namespace MonoEngine.Render
{
    public class RenderManager : DrawableGameComponent
    {
        public delegate void DrawCallBack();

        internal static SpriteBatch spriteBatch;

        private List<RenderManager.DrawCallBack> CallBack_Model = new List<RenderManager.DrawCallBack>();
        private List<RenderManager.DrawCallBack> CallBack_Sprite = new List<RenderManager.DrawCallBack>();
        private List<RenderManager.DrawCallBack> CallBack_Text = new List<RenderManager.DrawCallBack>();
        private List<RenderManager.DrawCallBack> CallBack_UIModel = new List<RenderManager.DrawCallBack>();
        private List<RenderManager.DrawCallBack> CallBack_RenderTarget = new List<RenderManager.DrawCallBack>();

        private static RenderManager instance;
        public static RenderManager Instance(Microsoft.Xna.Framework.Game game, SpriteBatch spiteBatch)
        {
            instance = (instance == null) ? new RenderManager(game) : instance;
            RenderManager.spriteBatch = spiteBatch;
            return instance;
        }

        public RenderManager(Microsoft.Xna.Framework.Game game) : base(game)
        {
            CallBack_Model = new List<DrawCallBack>();
            CallBack_Sprite = new List<DrawCallBack>();
            CallBack_Text = new List<DrawCallBack>();
            CallBack_UIModel = new List<DrawCallBack>();
            CallBack_RenderTarget = new List<DrawCallBack>();
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
        public static void RegisterDrawCallBack(ModelRenderer renderer, RenderManager.DrawCallBack callback)
        {
            instance.CallBack_Model.Add(callback);
        }

        public static void UnRegisterDrawCallBack(ModelRenderer renderer, RenderManager.DrawCallBack callback)
        {
            instance.CallBack_Model.Remove(callback);
        }

        public static void RegisterDrawCallBack(SpriteRenderer renderer, RenderManager.DrawCallBack callback)
        {
            instance.CallBack_Sprite.Add(callback);
        }

        public static void UnRegisterDrawCallBack(SpriteRenderer renderer, RenderManager.DrawCallBack callback)
        {
            instance.CallBack_Sprite.Remove(callback);
        }

        public static void RegisterDrawCallBack(TextRenderer renderer, RenderManager.DrawCallBack callback)
        {
            instance.CallBack_Text.Add(callback);
        }

        public static void UnRegisterDrawCallBack(TextRenderer renderer, RenderManager.DrawCallBack callback)
        {
            instance.CallBack_Text.Remove(callback);
        }

        public static void RegisterDrawCallBack(RenderTargetRenderer renderer, RenderManager.DrawCallBack callback)
        {
            instance.CallBack_RenderTarget.Add(callback);
        }

        public static void UnregisterDrawCallBack(RenderTargetRenderer renderer, RenderManager.DrawCallBack callback)
        {
            instance.CallBack_RenderTarget.Remove(callback);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (DrawCallBack draw in CallBack_Model)
            {
                draw();
            }

            spriteBatch.Begin();

            foreach (DrawCallBack draw in CallBack_Sprite)
            {
                draw();
            }

            foreach (DrawCallBack draw in CallBack_Text)
            {
                draw(); 
            }

            foreach (DrawCallBack draw in CallBack_RenderTarget)
            {
                draw();
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        

    }
}
