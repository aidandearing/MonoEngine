using System.Collections.Generic;
﻿using System;
using System.Reflection;

namespace MonoEngine.Game
{
    public class GameObject
    {
        /// <summary>
        /// The template object this object comes from in Resources
        /// </summary>
        public string template;

        public GameObject parent;
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
            component.parent = this;
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

        public void Broadcast(object obj, string methodName)
        {
            if (parent == null)
            {
                object[] objs = new object[1];
                objs[0] = obj;

                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(methodName);
                if (theMethod != null)
                    theMethod.Invoke(this, objs);

                foreach (GameObject component in components)
                {
                    component.MessageToChildren(obj, methodName);
                }
            }
            else
            {
                parent.Broadcast(obj, methodName);
            }
        }

        public void Broadcast(object[] obj, string methodName)
        {
            if (parent == null)
            {
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(methodName);
                if (theMethod != null)
                    theMethod.Invoke(this, obj);

                foreach (GameObject component in components)
                {
                    component.MessageToChildren(obj, methodName);
                }
            }
            else
            {
                parent.Broadcast(obj, methodName);
            }
        }

        public void Message(object obj, string methodName)
        {
            object[] objs = new object[1];
            objs[0] = obj;

            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName);
            if (theMethod != null)
                theMethod.Invoke(this, objs);
        }

        public void Message(object[] obj, string methodName)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName);
            if (theMethod != null)
                theMethod.Invoke(this, obj);
        }

        public void MessageToChildren(object obj, string methodName)
        {
            object[] objs = new object[1];
            objs[0] = obj;

            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName);
            if (theMethod != null)
                theMethod.Invoke(this, objs);

            foreach (GameObject component in components)
            {
                component.Message(obj, methodName);
            }
        }

        public void MessageToChildren(object[] obj, string methodName)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName);
            if (theMethod != null)
                theMethod.Invoke(this, obj);

            foreach (GameObject component in components)
            {
                component.Message(obj, methodName);
            }
        }
    }
}
