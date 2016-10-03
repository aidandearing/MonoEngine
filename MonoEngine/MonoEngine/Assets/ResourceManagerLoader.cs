using System;
using System.Collections.Generic;
using MonoEngine.Game;

namespace MonoEngine.Assets
{
    /// <summary>
    /// This abstract class contains an overrideable LoadAsset method which offers extensable loading of resources not generically supported by the engine
    /// If you are attempting to make a new kind of resource, and would like Resources to manage it for you, which is highly recommended, 
    /// create a class that inherits from this, and define how the asset must be loaded
    /// </summary>
    public abstract class ResourceManagerLoader
    {
        public static Dictionary<Type, ResourceManagerLoader> loaders = new Dictionary<Type, ResourceManagerLoader>();

        protected Type type;

        public ResourceManagerLoader(Type t)
        {
            loaders.Add(t, this);
            type = t;
        }

        public abstract object LoadAsset(string path, string name, Scene parent);
    }
}
