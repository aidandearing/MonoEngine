using System;
using System.Xml;
using MonoEngine.Assets;

namespace MonoEngine.Game
{
    public class Scene
    {
        public string Name { get; private set; }

        public SceneAssetsPackage assets;

        internal Scene(XmlReader reader)
        {
            assets = new SceneAssetsPackage(this);

            // Read down the xml and load all the references, via their xml based constructors
            // This should end with everything the scene says it needs, and where it needs it, alive and where it needs it (bad sentence structure is bad)
            // Which means we need to start making sure everything can build itself from XML, if it is part of a scene, so all GameObjects, all UI, etc.
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name.ToLower())
                    {
                        case "name":
                            // Give this scene a name
                            if (reader.Read())
                                Name = reader.Value;
                            break;
                        case "ref":
                            Type type = null;
                            type = Type.GetType(reader["type"], true, true);
                            Resources.LoadAsset(type, reader["name"], this);
                            break;
                    }
                }
            }
        }
    }
}
