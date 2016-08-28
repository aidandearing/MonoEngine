using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoEngine.Game
{
    class SceneManager : GameComponent
    {
        private Dictionary<string, Scene> scenes;

        private static SceneManager instance;
        public static SceneManager Instance(Microsoft.Xna.Framework.Game game)
        {
            instance = (instance == null) ? new SceneManager(game) : instance;

            return instance;
        }

        private SceneManager(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }

        // Scenes need to be retrieved from a .scene file (xml) in the Assets/Scenes/ directory

        // Scenes need to be loaded to the game (1 scene is loaded at any one time)

        // Scenes need to be switched (ideally in a way that doesn't unload any stuff that already is loaded, and wants to be used in the next scene too)

        // Scenes need to be unloaded when the game ends
    }
}
