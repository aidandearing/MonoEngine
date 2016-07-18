using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoEngine.Game
{
    public class GameObjectManager : DrawableGameComponent
    {
        private List<GameObject> gameObjects;
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
    }
}
