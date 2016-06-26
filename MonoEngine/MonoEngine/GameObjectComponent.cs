namespace Capstone
{
    /// <summary>
    /// The abstract base class for all GameObjectComponents
    /// </summary>
    abstract class GameObjectComponent
    {
        public GameObject parent;

        public GameObjectComponent(GameObject parent)
        {
            this.parent = parent;
        }
    }
}
