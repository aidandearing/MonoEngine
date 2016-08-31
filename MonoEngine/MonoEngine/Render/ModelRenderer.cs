using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoEngine.Assets;
using MonoEngine.Game;
using MonoEngine.Physics;

namespace MonoEngine.Render
{
    public class ModelRenderer : GameObject
    {
        private Model model;

        internal ModelRenderer(string name, Model model) : base(name)
        {
            this.model = model;
            RenderManager.RegisterDrawCallBack(this, new RenderManager.DrawCallBack(Draw));
        }

        public void Draw()
        {
            //model.Draw(Physics.WorldToRender(Camera.Transformation.Transformation + parent.transform.Transformation), Camera.View, Camera.Projection);
            model.Draw(PhysicsEngine.WorldToRender(Camera.MainCamera.transform.Transformation + transform.Transformation), Camera.MainCamera.View, Camera.MainCamera.Projection);
        }

        public static ModelRenderer MakeModelRenderer(string name)
        {
            return new ModelRenderer(name, Resources.LoadModel(name, SceneManager.activeScene));
        }
    }
}
