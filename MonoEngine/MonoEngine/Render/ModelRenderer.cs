using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Assets;
using MonoEngine.Game;
using MonoEngine.Physics;

namespace MonoEngine.Render
{
    public class ModelRenderer : GameObject
    {
        public Material material;

        private ModelWrapper model;
        public ModelWrapper Model
        {
            get
            {
                return model;
            }
        }

        internal ModelRenderer(string name, ModelWrapper model, string targetName = null) : base(name)
        {
            this.model = model;

            if (targetName == null)
            {
                RenderManager.RegisterDrawCallback("default", new RenderTargetBatch.DrawCallback(Draw));
            }
            else
            {
                RenderManager.RegisterDrawCallback(targetName, new RenderTargetBatch.DrawCallback(Draw));
            }
        }

        public void Draw()
        {
            model.SetEffect(material);
            //model.Draw(Physics.WorldToRender(Camera.Transformation.Transformation + parent.transform.Transformation), Camera.View, Camera.Projection);
            ((Model)model).Draw(PhysicsEngine.WorldToRender(Camera.MainCamera.transform.Transformation + transform.Transformation), Camera.MainCamera.View, Camera.MainCamera.Projection);
        }

        public static ModelRenderer MakeModelRenderer(string name)
        {
            return new ModelRenderer(name, Resources.LoadAsset(new ModelWrapper().GetType(), name, SceneManager.activeScene) as ModelWrapper);
        }
    }
}
