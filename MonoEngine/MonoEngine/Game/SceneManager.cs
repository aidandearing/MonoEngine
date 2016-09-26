using System.Collections.Generic;
using System.Xml;
using MonoEngine.Assets;
using Microsoft.Xna.Framework;

namespace MonoEngine.Game
{
    public class SceneManager : GameComponent
    {
        public static Scene activeScene;

        private static SceneManager instance;
        public static SceneManager Instance(Microsoft.Xna.Framework.Game game)
        {
            instance = (instance == null) ? new SceneManager(game) : instance;

            return instance;
        }

        private SceneManager(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }
        
        // TODO: MAKE THIS THREADED!
        public static void LoadScene(string name)
        {
            // Scenes can be found at Assets/Scenes/ directory as .scene files (xml)
            // Scenes need to be loaded to the game (1 scene is loaded at any one time)
            // Scenes handle the actual loading, this just starts the process
            Scene newScene = null;

            // I should probably start the whole xml process here, pass the reader to the Scene and let it start cascading off to get everything set up
            using (XmlReader reader = XmlReader.Create(@"./Content/Scenes/" + name + ".scene"))
            {
                newScene = new Scene(reader);
            }

            // The last step of scene loading is to ensure that old assets are removed via the UnLoadScene method in Resources
            Resources.UnloadScene(newScene);
        }
        
        // Scenes need to be unloaded when the game ends
        public static void UnloadScene()
        {
            activeScene = null;
        }
    }
}
