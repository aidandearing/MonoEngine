using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoEngine.Game;

namespace MonoEngine
{
    namespace Render
    {
        public class ModelRenderer : GameObject
        {
            private static Dictionary<string, Model> models = new Dictionary<string, Model>();

            private Model model;

            private ModelRenderer(string name, Model model) : base(name)
            {
                this.model = model;
            }

            public override void Render()
            {
                //model.Draw(Physics.WorldToRender(Camera.Transformation.Transformation + parent.transform.Transformation), Camera.View, Camera.Projection);
                model.Draw((Camera.MainCamera.transform.Transformation + transform.Transformation) * 200.0f, Camera.MainCamera.View, Camera.MainCamera.Projection);
            }

            /// <summary>
            /// Factory Method Pattern
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public static ModelRenderer MakeModelRenderer(GameObject parent, string name)
            {
                // If the model isn't already loaded (it's key isn't found in the dictionary)
                if (!models.ContainsKey(name))
                {
                    // Needs to try to get the model at that name in the models path & load it
                    Model model = ContentHelper.Content.Load<Model>("Assets/Models/" + name);
                    // add the model into the dictionary
                    models.Add(name, model);
                }
                // Pass the model at that key in the dictionary
                return new ModelRenderer(name, models[name]);
            }
        }
    }
}
