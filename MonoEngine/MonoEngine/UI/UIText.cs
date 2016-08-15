using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Render;

namespace MonoEngine.UI
{
    class UIText : UIObject
    {
        public int fontSize;
        public string text;
        //public 

        public UIText(string name, Rectangle bounds, UIAlignment boundsAlign, UIAlignment alignment, flags flag) : base(name, bounds, boundsAlign, alignment, flag)
        {

        }
    }
}
