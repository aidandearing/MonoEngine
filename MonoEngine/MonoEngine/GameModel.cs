using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Capstone
{
    class GameModel : GameObjectComponent, IGameObjectRenderable
    {
        private static Dictionary<string, Model> models = new Dictionary<string, Model>();

        private Model model;

        public GameModel(GameObject parent, Model model) : base(parent)
        {
            this.model = model;
        }

        void IGameObjectRenderable.Render()
        {
            //model.Draw(Physics.WorldToRender(Camera.Transformation.Transformation + parent.transform.Transformation), Camera.View, Camera.Projection);
            model.Draw(Physics.WorldToRender(Camera.MainCamera.parent.transform.Transformation + parent.transform.Transformation), Camera.MainCamera.View, Camera.MainCamera.Projection);
        }

        public static GameModel MakeGameModel(GameObject parent, string name)
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
            return new GameModel(parent, models[name]);
        }
    }
}
