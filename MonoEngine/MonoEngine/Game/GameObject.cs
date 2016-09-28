using System.Collections.Generic;
using System;
using System.Reflection;
using System.Xml;

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

            template = "";
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

        // Reflection based method calling by name on scene tree

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

        internal static GameObject LoadFromXML(string path)
        {
            GameObject obj = null;
            return obj;
        }

        public void LoadToXML()
        {
            // This is where all the other spooky reflection magic happens
            string name = this.GetType().Name;

            // Get the field info from both this instance, and it's parent instance
            FieldInfo[] infos = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\n";

            // Start by creating an XMLWriter
            using (XmlWriter writer = XmlWriter.Create(@"Content/Assets/Objects/" + name + ".prefab", settings))
            {
                writer.WriteStartElement(name);

                foreach (FieldInfo info in infos)
                {
                    writer.WriteStartElement(info.Name);
                    writer.WriteAttributeString("type", info.FieldType.ToString());
                    object val = info.GetValue(this);
                    if (val != null)
                    {
                        writer.WriteRaw(val.ToString());
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            // Write the start node with name of this class
            // Then for every parameter write its name as the node, type of the variable, then its value is the value of the parameter
            // End (easy, lol)

            //base.LoadToXML();
        }
    }
}
