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
    public class UIObject
    {
        public enum flags { None = 0, Focusable = 1, Selectable = 2, Focusable_Controller = 4, Player0 = 8, Player1 = 16, Player2 = 32, Player3 = 64, Player4 = 128, Player5 = 256, Player6 = 512, Player7 = 1024 };

        public flags flag;
        public UIObject parent;
        public List<UIObject> childObjects;
        public Rectangle bounds;
        public Vector2 origin;
        public UIObject previousObj;
        public UIObject nextObj;

        public string Name { get; set; }

        public UIObject(string name, Rectangle bounds, UIAlignment boundsAlign, UIAlignment alignment, flags flag)
        {
            childObjects = new List<UIObject>();
            this.Name = name;
            this.bounds = bounds;
            this.flag = flag;

            Vector2 boundsAlignment = boundsAlign.GetAlignment(this, parent);
            this.bounds.X = (int)boundsAlignment.X;
            this.bounds.Y = (int)boundsAlignment.Y;

            origin = alignment.GetAlignment(this, this);
        }
        public UIObject(string name, UIAlignment boundsAlign, UIAlignment alignment, flags flag)
        {
            bounds = new Rectangle(0, 0, 0, 0);
            childObjects = new List<UIObject>();
            this.Name = name;
            this.flag = flag;

            Vector2 boundsAlignment = boundsAlign.GetAlignment(this, parent);
            bounds.X = (int)boundsAlignment.X;
            bounds.Y = (int)boundsAlignment.Y;

            origin = alignment.GetAlignment(this, this);
        }

        public void Add(UIObject obj)
        {
            obj.parent = this;
            //the previous object is the last one in the list
            obj.previousObj = childObjects.Last();
            //the next object is the current object being added
            childObjects.Last().nextObj = obj;
            childObjects.Add(obj);
        }

        public void Remove(UIObject obj)
        {
            childObjects.Remove(obj);
        }

        public UIObject GetUIObjectByName(string name)
        {
            
            if (name == this.Name)
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
            