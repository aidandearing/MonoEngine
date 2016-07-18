using System.Collections.Generic;
using MonoEngine.Render;

namespace MonoEngine.Game
{
    public class GameObject
    {
        /// <summary>
        /// This is the list of all components this GameObject has
        /// Components are where GameObjects get their behaviours
        /// </summary>
        private List<GameObject> components;

        /// <summary>
        /// Adds a component to the GameObject
        /// </summary>
        /// <param name="component">The component to be added</param>
        public void AddComponent(GameObject component)
        {
            components.Add(component);
            component.transform.parent = this.transform;
        }

        public Transform transform;

        public string Name { get; set; }

        public GameObject(string name)
        {
            transform = new Transform();
            Name = name;

            components = new List<GameObject>();
        }

        public virtual void Update()
        {
            foreach (GameObject component in components)
            {
                component.Update();
            }
        }

        public GameObject GetComponent<T>()
        {
            foreach (GameObject component in components)
            {
                if (component is T)
                    return component;
            }

            return null;
        }

        public List<GameObject> GetComponents<T>()
        {
            List<GameObject> returnComponents = new List<GameObject>();

            foreach (GameObject component in components)
            {
                if (component is T)
                    returnComponents.Add(component);
            }

            return returnComponents;
        }
    }
}
