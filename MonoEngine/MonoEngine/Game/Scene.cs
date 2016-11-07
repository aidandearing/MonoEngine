using System.Xml;
using MonoEngine.Assets;

namespace MonoEngine.Game
{
    public class Scene
    {
        public SceneAssetsPackage assets;

        public Scene()
        {
            assets = new SceneAssetsPackage(this);
        }

        internal Scene (XmlReader reader)
        {
            assets = new SceneAssetsPackage(this);
            // Read down the xml and load all the references, via their xml based constructors
            // This should end with everything the scene says it needs, alive and where it needs it
            // Which means we need to start making sure everything can build itself from XML, if it is part of a scene, so all GameObjects, all UI, etc.
        }

        public virtual void Update()
        {

        }
    }
}