using System;
using MonoEngine.Game;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Assets
{
    class LoaderModel : ResourceManagerLoader
    {
        public LoaderModel(Type type) : base(type) { }

        public override object LoadAsset(string path, string name, Scene parent)
        {
            // Load a model from a name at the Assets/Models/ + name.xnb pathwa
            // Needs to try to get the model at that name in the models path & load it
            Model model = ContentHelper.Content.Load<Model>("Assets/Models/" + name);

            if (parent != null)
            {
                parent.assets.assets[typeof(Model)].Add(name);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            return model;
        }
    }
}
