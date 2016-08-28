using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoEngine.Game
{
    public class SceneManager : GameComponent
    {
        private Scene activeScene;

        private static SceneManager instance;
        public static SceneManager Instance(Microsoft.Xna.Framework.Game game)
        {
            instance = (instance == null) ? new SceneManager(game) : instance;

            return instance;
        }

        private SceneManager(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }
        
        public static void LoadScene(string name)
        {
            // Scenes can be found at Assets/Scenes/ directory as .scene files (xml)
            // Scenes need to be loaded to the game (1 scene is loaded at any one time)
            // Scenes handle the actual loading, this just starts the process

            // I should probably start the whole xml process here, pass the reader to the Scene and let it start cascading off to get everything set up

            // Scenes need to be switched (ideally in a way that doesn't unload any stuff that already is loaded, and wants to be used in the next scene too)
            // So whenever activeScene != null this switching logic needs to occur

            // This can probably best be done by first loading the new scene into existance, in a temporary variable
            // Then killing the old scene (this way anything in the new temporary scene that references something already loaded in resources keeps the reference, and Garbage Collection can do the rest)
            // Kill the old scene by making the activeScene = the temporary new scene
            // Garbage Collection does the rest!
            // Which I will admit terrifies me a bit, what if it is slower than doing it myself, I doubt it though,
            // All those smart people making the std and stuffs, well not them actually, they are C++ but I refer 
            // To the capacities of people like them which I imagine Microsoft would have employed similarly to 
            // Construct the Garbage Collection system behind C# and other safe languages

            // Silly me. Resources has references to everything ever loaded. It doesn't matter if the scene goes and the ModelRenderer using a model goes, the Resources class still has that Model referenced in its own dictionary, and therefore Garbage Collection will never remove it.
            // Either I need a way of being able to trace references so that I can test if something being referenced by Resources is still being referenced elsewhere (no easy task)
            // Or I still need to construct a method that manually checks all the references in the new scene vs the old scene and loads the new references, keeps the overlaps, and forgets the old references.
        }
        
        // Scenes need to be unloaded when the game ends
        public static void UnloadScene()
        {
            instance.activeScene = null;
            // More Garbage Collection magic. Uhhhh, refer to the above paragraph.
        }
    }
}
