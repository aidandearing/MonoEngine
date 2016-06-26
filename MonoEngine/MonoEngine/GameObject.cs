using System.Collections.Generic;

namespace MonoEngine
{
    namespace Game
    {
        public class GameObject
        {
            public GameObject parent;
            /// <summary>
            /// This is the list of all components this GameObject has
            /// Components are where GameObjects get their behaviours
            /// </summary>
            private List<GameObject> components;
            private List<IGameObjectUpdatable> components_updatable;
            private List<IGameObjectRenderable> components_renderable;

            /// <summary>
            /// Adds a component to the GameObject
            /// </summary>
            /// <param name="component">The component to be added</param>
            public void AddComponent(GameObject component)
            {
                components.Add(component);
                component.parent = this;

                if (component is IGameObjectUpdatable)
                    components_updatable.Add(component as IGameObjectUpdatable);

                if (component is IGameObjectRenderable)
                    components_renderable.Add(component as IGameObjectRenderable);
            }

            public Transform transform;

            public string Name { get; set; }

            public GameObject(string name)
            {
                transform = new Transform();
                Name = name;

                components = new List<GameObject>();
                components_updatable = new List<IGameObjectUpdatable>();
                components_renderable = new List<IGameObjectRenderable>();
            }

            public void Update()
            {
                foreach (IGameObjectUpdatable component in components_updatable)
                {
                    component.Update();
                }
            }

            public void Render()
            {
                foreach (IGameObjectRenderable component in components_renderable)
                {
                    component.Render();
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
}
