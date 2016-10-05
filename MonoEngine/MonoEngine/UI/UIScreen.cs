using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.UI
{
    public class UIScreen
    {
        public List<UIObject> rootObjects;

        public UIScreen()
        {
            rootObjects = new List<UIObject>();
        }

        public static UIScreen MakeScreen(string path)
        {
            return null;
        }

        public UIObject GetUIObjectByName(string name)
        {
            UIObject obj = null;

            foreach (UIObject root in rootObjects)
            {
                obj = root.GetUIObjectByName(name);

                if (obj != null)
                {
                    return obj;
                }
            }
            return obj;
        }

        public void AddUIObjectToUIObjectByName(string name, UIObject obj)
        {
            UIObject namedObj = GetUIObjectByName(name);

            if (namedObj != null)
            {
                namedObj.AddUIObject(obj);
            }
        }

        public void RemoveUIObjectFromUIObjectByName(string name, UIObject obj)
        {
            UIObject namedObj = GetUIObjectByName(name);

            if (namedObj != null)
            {
                namedObj.RemoveUIObject(obj);
            }
        }
    }
}
