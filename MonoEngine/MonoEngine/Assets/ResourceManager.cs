using System;
using System.Collections.Generic;
using MonoEngine.Game;

namespace MonoEngine.Assets
{
    public abstract class ResourceMetaData
    {
        protected string path;
        public string Path
        {
            get { return path; }
            private set { path = value; }
        }

        internal ResourceMetaData(string path)
        {
            this.path = path;
        }

        public abstract bool ContainsResource(string name);

        public abstract void RemoveResource(string name);
    }

    public class ResourceManager<T> : ResourceMetaData
    {
        private Dictionary<string, T> dictionary;

        internal ResourceManager(string path) : base(path)
        {
            dictionary = new Dictionary<string, T>();
        }

        public void AddResource(string name, T asset)
        {
            dictionary.Add(name, asset);
        }

        public override void RemoveResource(string name)
        {
            dictionary.Remove(name);
        }

        public override bool ContainsResource(string name)
        {
            return dictionary.ContainsKey(name);
        }

        public T GetResource(string name)
        {
            return dictionary[name];
        }
    }
}
