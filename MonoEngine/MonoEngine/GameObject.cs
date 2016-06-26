using System.Collections.Generic;

namespace Capstone
{
    class GameObject
    {
        /// <summary>
        /// This is the list of all components this GameObject has
        /// Components are where GameObjects get their behaviours
        /// </summary>
        private List<GameObjectComponent> components;
        private List<IGameObjectUpdatable> components_updatable;
        private List<IGameObjectRenderable> components_renderable;

        /// <summary>
        /// Adds a component to the GameObject
        /// </summary>
        /// <param name="component">The component to be added</param>
        public void AddComponent(GameObjectComponent component)
        {
            components.Add(component);

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

            components = new List<GameObjectComponent>();
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

        public GameObjectComponent GetComponent<T>()
        {
            foreach (GameObjectComponent component in components)
            {
                if (component is T)
                    return component;
            }

            return null;
        }

        public List<GameObjectComponent> GetComponents<T>()
        {
            List<GameObjectComponent> returnComponents = new List<GameObjectComponent>();

            foreach (GameObjectComponent component in components)
            {
                if (component is T)
                    returnComponents.Add(component);
            }

            return returnComponents;
        }
    }
}
