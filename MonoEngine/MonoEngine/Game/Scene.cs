using System.Xml;

namespace MonoEngine.Game
{
    public class Scene
    {
        internal Scene (XmlReader reader)
        {
            // Read down the xml and load all the references, via their xml based constructors
            // This should end with everything the scene says it needs, and where it needs it, alive and where it needs it (bad sentence structure is bad)
            // Which means we need to start making sure everything can build itself from XML, if it is part of a scene, so all GameObjects, all UI, etc.
        }
    }
}
