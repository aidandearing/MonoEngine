using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoEngine.Game
{
    public class GameObjectManager : DrawableGameComponent
    {
        private List<GameObject> gameObjects;
        private Dictionary<string, GameObject> lookup;
        private List<GameObject> gameObjectsDead;

        private static GameObjectManager instance;
        public static GameObjectManager Instance(Microsoft.Xna.Framework.Game game)
        {

            instance = (instance == null) ? new GameObjectManager(game) : instance;

            return instance;
        }

        private GameObjectManager(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }

        public static void AddGameObject(GameObject obj)
        {
            instance.gameObjects.Add(obj);
            instance.lookup.Add(obj.Name, obj);
        }

        public static void RemoveGameObject(GameObject obj)
        {
            instance.gameObjectsDead.Add(obj);
        }

        public override void Initialize()
        {
            base.Initialize();

            gameObjects = new List<GameObject>();

            gameObjectsDead = new List<GameObject>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (GameObject obj in gameObjects)
            {
                obj.Update();
            }
            foreach (GameObject obj in gameObjectsDead)
            {
                gameObjects.Remove(obj);
                lookup.Remove(obj.Name);
            }
            gameObjectsDead.Clear();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //foreach (GameObject obj in gameObjects)
            //{
            //    obj.Draw();
            //}
        }

        /// <summary>
        /// Finds the first game object of a specific type
        /// </summary>
        /// <typeparam name="T">The type of game object being looked for</typeparam>
        /// <returns>Null or the found object</returns>
        public static GameObject GetGameObject<T>()
        {
            foreach (GameObject obj in instance.gameObjects)
            {
                if (obj is T)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// Finds all game objects of a specific type
        /// </summary>
        /// <typeparam name="T">The type of game object being looked for</typeparam>
        /// <returns>An empty list or all instances of a specific type</returns>
        public static List<GameObject> GetGameObjects<T>()
        {
            List<GameObject> objs = new List<GameObject>();

            foreach (GameObject obj in instance.gameObjects)
            {
                if (obj is T)
                    objs.Add(obj);
            }

            return objs;
        }

        /// <summary>
        /// Finds all game objects that come from a Resources template
        /// </summary>
        /// <param name="name">The name of the template</param>
        /// <returns>An empty list or all instances of the template</returns>
        public static List<GameObject> GetGameObjectsByTemplate(string name)
        {
            List<GameObject> objs = new List<GameObject>();

            foreach(GameObject obj in instance.gameObjects)
            {
                if (obj.template == name)
                    objs.Add(obj);
            }

            return objs;
        }

        /// <summary>
        /// Finds the first game object of a specific name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject GetGameObjectByName(string name)
        {
            if (instance.lookup.ContainsKey(name))
                return instance.lookup[name];

            return null;
        }

        /// <summary>
        /// Finds all game objects that contain the name string in their name
        /// </summary>
        /// <param name="name">The string they must have in their name</param>
        /// <returns>A list of all game objects that meet the name contain criteria</returns>
        public static List<GameObject> GetGameObjectsByName(string name)
        {
            List<GameObject> objs = new List<GameObject>();

            foreach (GameObject obj in instance.gameObjects)
            {
                if (obj.Name.Contains(name))
                    objs.Add(obj);
            }

            return objs;
        }
    }
}
