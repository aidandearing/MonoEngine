using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Assets
{
    public class ModelWrapper
    {
        public Model Model { get; private set; }

        public ModelWrapper() { }

        public ModelWrapper(Model model)
        {
            Model = model;
        }

        public Effect[] GetEffect()
        {
            List<Effect> effects = new List<Effect>();

            foreach(ModelMesh mesh in Model.Meshes)
            {
                foreach(Effect effect in mesh.Effects)
                {
                    effects.Add(effect);
                }
            }

            return effects.ToArray();
        }

        public void SetEffect(Effect effect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }
        }

        public static implicit operator Model(ModelWrapper _model)
        {
            return _model.Model;
        }
    }
}
