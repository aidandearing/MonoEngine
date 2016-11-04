﻿using System;
using System.Xml;
using MonoEngine.Game;

namespace MonoEngine.Assets
{
    class LoaderGameObject : ResourceManagerLoader
    {
        public LoaderGameObject(Type t) : base(t) { }

        public override object LoadAsset(string path, string name, Scene parent)
        {
            // Load a resource from a file at the path + name.xnb pathway
            GameObject asset = new GameObject();
            // If the model isn't already loaded (it's key isn't found in the dictionary)
            // Needs to try to get the model at that name in the models path & load it
            //using (XmlReader reader = XmlReader.Create(@"./Content/" + path + "/" + name + ".prefab"))
            //{
            //    while (reader.Read())
            //    {
            //        if (reader.IsStartElement())
            //            GameObject.LoadFromXML(reader);
            //    }
            //}

            //XmlReader reader = XmlReader.Create(@"./Content/" + path + "/" + name + ".prefab");
            //System.Xml.Serialization.XmlSerializer cereal = new System.Xml.Serialization.XmlSerializer(asset.GetType());
            //asset = cereal.Deserialize(reader) as GameObject;

            asset = Resources.Deserialise(@"./Content/" + path + "/" + name + ".prefab") as GameObject;

            if (parent != null)
            {
                parent.assets.AddAsset(name, type);
            }
            else
            {
                // TODO: Log a warning that unbound assets will not unload when a scene switch occurs
            }

            return asset;
        }
    }
}
