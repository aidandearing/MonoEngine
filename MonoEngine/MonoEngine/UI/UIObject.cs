using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Render;

namespace MonoEngine.UI
{
    public abstract class UIObject
    {
        public List<UIObject> childObjects;
        public Rectangle bounds;
        //this must be a unique name for each uiObject
        public string name;

        public UIObject(string name)
        {
            childObjects = new List<UIObject>();
            bounds = new Rectangle();
            this.name = name;
        }

        public void AddUIObject(UIObject obj)
        {
            childObjects.Add(obj);
        }

        public void RemoveUIObject(UIObject obj)
        {
            childObjects.Remove(obj);
        }

        public UIObject GetUIObjectByName(string name)
        {
            
            if (name == this.name)
            {
                return this;
            }
            else
            {
                UIObject obj = null;

                foreach (UIObject child in childObjects)
                {
                    obj = child.GetUIObjectByName(name);

                    if (obj != null)
                    {
                        return obj;
                    }
                }
                return obj;
            }
        }
    }
}
            