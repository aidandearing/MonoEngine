using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Assets
{
    [System.Serializable]
    public class ModelWrapper
    {
        private Model model;

        /// <summary>
        /// NEVER USE THIS, IT IS USED SPECIFICALLY TO GET THE RUNTIME TYPE OF AN INSTANCE OF FONT, AND NOTHING ELSE
        /// </summary>
        public ModelWrapper() { }

        public ModelWrapper(Microsoft.Xna.Framework.Graphics.Model model)
        {
            this.model = model;
        }

        public static implicit operator Model(ModelWrapper _model)
        {
            return _model.model;
        }
    }
}
