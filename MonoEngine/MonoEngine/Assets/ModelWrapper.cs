using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Assets
{
    public class ModelWrapper
    {
        private Model model;

        /// <summary>
        /// NEVER USE THIS, IT IS USED SPECIFICALLY TO GET THE RUNTIME TYPE OF AN INSTANCE OF FONT, AND NOTHING ELSE
        /// </summary>
        internal ModelWrapper() { }

        public ModelWrapper(Model model)
        {
            this.model = model;
        }

        public void SetEffect(Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }
        }

        public static implicit operator Model(ModelWrapper _model)
        {
            return _model.model;
        }
    }
}
