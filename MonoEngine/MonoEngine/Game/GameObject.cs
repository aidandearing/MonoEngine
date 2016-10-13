using System.Collections.Generic;
using System;
using System.Reflection;
using System.Xml;
using MonoEngine.Assets;
using MonoEngine.Shapes;

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

        /// <summary>
        /// This constructor does nothing, and is only used to create instances of GameObjects in order to get their type
        /// </summary>
        internal GameObject() { }

        public GameObject(string name)
        {
            transform = new Transform();
            Name = name;

            components = new List<GameObject>();

            template = "";

            GameObjectManager.AddGameObject(this);
        }

        public virtual void Update()
        {
            foreach (GameObject component in components)
            {
                component.Update();
            }
        }

        public GameObject GetComponent(Type type)
        {
            foreach (GameObject component in components)
            {
                if (component.GetType() == type)
                    return component;
            }

            return null;
        }

        public List<GameObject> GetComponents(Type type)
        {
            List<GameObject> returnComponents = new List<GameObject>();

            foreach (GameObject component in components)
            {
                if (component.GetType() == type)
                    returnComponents.Add(component);
            }

            return returnComponents;
        }

        internal void PhysicsBody2DCallStart(Physics.Physics2D.Collision2D collision)
        {
            if (this is Physics.Physics2D.IPhysicsListener2D)
            {
                (this as Physics.Physics2D.IPhysicsListener2D).OnCollision2DStart(collision);
            }

            foreach (GameObject component in components)
            {
                if (component is Physics.Physics2D.IPhysicsListener2D)
                {
                    (component as Physics.Physics2D.IPhysicsListener2D).OnCollision2DStart(collision);
                }
            }
        }

        internal void PhysicsBody2DCallStay(Physics.Physics2D.Collision2D collision)
        {
            if (this is Physics.Physics2D.IPhysicsListener2D)
            {
                (this as Physics.Physics2D.IPhysicsListener2D).OnCollision2DStay(collision);
            }

            foreach (GameObject component in components)
            {
                if (component is Physics.Physics2D.IPhysicsListener2D)
                {
                    (component as Physics.Physics2D.IPhysicsListener2D).OnCollision2DStay(collision);
                }
            }
        }

        internal void PhysicsBody2DCallStop(Physics.Physics2D.Collision2D collision)
        {
            if (this is Physics.Physics2D.IPhysicsListener2D)
            {
                (this as Physics.Physics2D.IPhysicsListener2D).OnCollision2DStop(collision);
            }

            foreach (GameObject component in components)
            {
                if (component is Physics.Physics2D.IPhysicsListener2D)
                {
                    (component as Physics.Physics2D.IPhysicsListener2D).OnCollision2DStop(collision);
                }
            }
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

        public override string ToString()
        {
            return Name;
        }

        internal static GameObject LoadFromXML(string path)
        {
            GameObject obj = new GameObject();
            
            using (XmlReader reader = XmlReader.Create(@path))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name.ToLower())
                        {
                            case "name":
                                // This thing got a name? Yeah, fab, give it the name then
                                if (reader.Read())
                                    obj.Name = reader.Value;
                                break;
                            case "template":
                                // What is this GameObject a template from? Can we go get that instead and just replace this with a clone of it?
                                if (reader.Read())
                                    obj.template = reader.Value;
                                break;
                            case "transform":
                                // What is the transform of this object
                                obj.transform = Transform.XmlToTransform(reader);
                                break;
                            case "parent":
                                // What is this GameObject's parent object
                                obj.parent = Resources.Ref(reader) as GameObject;
                                break;
                            case "ref":
                                // Refs are used when you are referencing another game object to be used as a component in this one
                                obj.AddComponent(Resources.Ref(reader) as GameObject);
                                break;
                            case "make":
                                // We need to make a new GameObject right here
                                obj.AddComponent(LoadFromXML(reader) as GameObject);
                                break;
                            case "find":
                                break;
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// This method is used whenever a ref game object uses a make tag to make another one
        /// </summary>
        /// <param name="reader">The reader that found the make tag</param>
        /// <returns>The newly made game object</returns>
        internal static GameObject LoadFromXML(XmlReader reader)
        {
            GameObject obj = null;

            int depth = reader.Depth;

            Type type = null;

            type = Type.GetType(reader["type"], true, true);

            Type[] types = new Type[1];
            types[0] = "".GetType();

            object[] parameters = new object[1];
            parameters[0] = reader["name"];

            //obj = type.GetConstructor(types).Invoke(parameters) as GameObject;
            obj = Activator.CreateInstance(type, parameters) as GameObject;

            FieldInfo[] infos = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            while (reader.Read() && reader.Depth > depth)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name.ToLower())
                    {
                        case "template":
                            // What is this GameObject a template from? Can we go get that instead and just replace this with a clone of it?
                            if (reader.Read())
                                obj.template = reader.Value;
                            break;
                        case "transform":
                            // What is the transform of this object
                            obj.transform = Transform.XmlToTransform(reader);
                            break;
                        case "parent":
                            // What is this GameObject's parent object
                            obj.parent = Resources.Ref(reader) as GameObject;
                            break;
                        case "ref":
                            // Refs are used when you are referencing another game object to be used as a component in this one
                            obj.AddComponent(Resources.Ref(reader) as GameObject);
                            break;
                        case "make":
                            // We need to make a new GameObject right here
                            obj.AddComponent(LoadFromXML(reader) as GameObject);
                            break;
                        case "find":
                            break;
                        case "shape":
                            if (obj is Physics.Physics2D.PhysicsBody2D)
                            {
                                (obj as Physics.Physics2D.PhysicsBody2D).shape = Shape.XMLToShape(reader);
                            }
                            break;
                        default:
                            foreach (FieldInfo info in infos)
                            {
                                //if (!info.GetType().IsEnum)
                                //{
                                    if (info.Name == reader.Name)
                                    {
                                        if (reader.Read())
                                            info.SetValue(obj, Convert.ChangeType(reader.Value, info.FieldType));
                                    }
                                //}
                            }
                            break;
                    }
                }
            }

            return obj;
        }

        public void LoadToXML()
        {
            // This is where all the other spooky reflection magic happens
            string name = this.GetType().FullName;

            // Get the field info from both this instance, and it's parent instance
            FieldInfo[] infos = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\n";

            // Start by creating an XMLWriter
            using (XmlWriter writer = XmlWriter.Create(@"Content/Assets/Objects/" + this.Name + ".prefab", settings))
            {
                // Write the start node with name of this class
                writer.WriteStartElement("object");
                writer.WriteAttributeString("type", name);
                writer.WriteAttributeString("name", this.Name);

                // Then for every parameter write its name as the node, type of the variable, then its value is the value of the parameter
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

                // Then for every parameter write its name as the node, type of the variable, then its value is the value of the parameter
                foreach (GameObject obj in components)
                {
                    writer.WriteStartElement("ref");
                    writer.WriteRaw(obj.Name);
                    writer.WriteEndElement();

                    obj.LoadToXML();
                }

                // End (easy, lol)
                writer.WriteEndElement();
            }
        }
    }
}
