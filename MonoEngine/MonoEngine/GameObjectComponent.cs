namespace MonoEngine
{
    namespace Game
    {
        /// <summary>
        /// The abstract base class for all GameObjectComponents
        /// </summary>
        public abstract class GameObjectComponent
        {
            public GameObject parent;

            public GameObjectComponent(GameObject parent)
            {
                this.parent = parent;
            }
        }
    }
}
