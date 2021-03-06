﻿using System;
using System.Collections.Generic;
using MonoEngine.Game;

namespace MonoEngine.Assets
{
    public class ResourceManager
    {
        public string Path { get; private set; }

        public ResourceManagerLoader Loader;

        private Type type;

        public Type TypeOfThis()
        {
            return type;
        }

        private Dictionary<string, object> dictionary;

        internal ResourceManager(string path, Type type, ResourceManagerLoader loader)
        {
            this.Path = path;
            this.Loader = loader;
            this.type = type;

            dictionary = new Dictionary<string, object>();
        }

        public void AddResource(string name, object asset)
        {
            dictionary.Add(name, asset);
        }

        public void RemoveResource(string name)
        {
            dictionary.Remove(name);
        }

        public bool ContainsResource(string name)
        {
            return dictionary.ContainsKey(name);
        }

        public object GetResource(string name)
        {
            return dictionary[name];
        }
    }
}
