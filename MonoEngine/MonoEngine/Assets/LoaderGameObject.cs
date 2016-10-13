using System;
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
            GameObject asset = null;
            // If the model isn't already loaded (it's key isn't found in the dictionary)
            // Needs to try to get the model at that name in the models path & load it
            using (XmlReader reader = XmlReader.Create(@"./Content/" + path + name + ".xml"))
            {
                int depth = reader.Depth;

                while (reader.Read() && depth < reader.Depth)
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "name":
                                if (reader.Read())
                                    asset.Name = reader.Value;
                                break;
                            case "transform":
                                // This is an optional xml node
                                asset.transform = Transform.XmlToTransform(reader);
                                break;
                            case "ref":
                                GameObject obj = Resources.Ref(reader) as GameObject;
                                asset.AddComponent(obj as GameObject);
                                break;
                        }
                    }
                }
            }

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
