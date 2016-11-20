using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Render;

namespace MonoEngine.UI
{
    public class UIAlignment
    {
        public enum Alignment { None, TopLeft, TopCenter, TopRight, CenterLeft, Center, CenterRight, BottomLeft, BottomCenter, BottomRight, Top, Left, Right, Bottom, FloatTop, FloatLeft, FloatRight, FloatBottom };
        public Alignment alignment;

        public UIAlignment(Alignment alignment)
        {
            this.alignment = alignment;
        }
        public UIAlignment()
        {

        }
        public Vector2 GetAlignment(UIObject obj, UIObject parent)
        {
            //if the object you are aligning doesnt have a parent we will assume the drawing bounds are the screen
            Rectangle other = GraphicsHelper.screen;

            Vector2 pos = Vector2.Zero;

            //if the object has a parent use the parents bounds instead
            if (parent != null)
            {
                other = parent.bounds;
            }

            switch (alignment)
            {
                case Alignment.Top:
                    pos.Y = other.Top;
                    break;
                case Alignment.TopLeft:
                    pos.X = other.Left;
                    pos.Y = other.Top;
                    break;
                case Alignment.TopCenter:
                    if (obj != parent)
                        pos.X = other.Center.X - (obj.bounds.Width / 2.0f);
                    else
                        pos.X = other.Center.X;
                    pos.Y = other.Top;
                    break;
                case Alignment.TopRight:
                    pos.X = other.Right - (obj.bounds.Width);
                    pos.Y = other.Top;
                    break;
                case Alignment.Left:
                    pos.X = other.Left;
                    break;
                case Alignment.CenterLeft:
                    pos.X = other.Left;
                    if (obj != parent)
                        pos.Y = other.Center.Y - (obj.bounds.Height / 2.0f);
                    else
                        pos.Y = other.Center.Y;
                    break;
                case Alignment.Center:
                    if (obj != parent)
                    {
                        pos.X = other.Center.X - (obj.bounds.Width / 2.0f);
                        pos.Y = other.Center.Y - (obj.bounds.Height / 2.0f);
                    }
                    else
                    {
                        pos.X = other.Center.X;
                        pos.Y = other.Center.Y;
                    }
                    break;
                case Alignment.CenterRight:
                    pos.X = other.Right - (obj.bounds.Width);
                    if (obj != parent)
                        pos.Y = other.Center.Y - (obj.bounds.Height / 2.0f);
                    else
                        pos.Y = other.Center.Y;
                    break;
                case Alignment.Right:
                    pos.X = other.Right;
                    break;
                case Alignment.Bottom:
                    pos.Y = other.Height;
                    break;
                case Alignment.BottomLeft:
                    pos.X = other.Left;
                    pos.Y = other.Bottom - (obj.bounds.Height);
                    break;
                case Alignment.BottomCenter:
                    if (obj != parent)
                        pos.X = other.Center.X - (obj.bounds.Width / 2.0f);
                    else
                        pos.X = other.Center.X;
                    pos.Y = other.Bottom - (obj.bounds.Height);
                    break;
                case Alignment.BottomRight:
                    pos.X = other.Right - (obj.bounds.Width);
                    pos.Y = other.Bottom - (obj.bounds.Height);
                    break;
                case Alignment.FloatLeft:
                    if (obj.previousObj != null)
                        pos.X = obj.previousObj.bounds.Right;
                    else
                        pos.X = other.Left;
                    break;
                case Alignment.FloatRight:
                    if (obj.previousObj != null)
                        pos.X = obj.previousObj.bounds.Left;
                    else
                        pos.X = other.Right;
                    break;
                case Alignment.FloatTop:
                    if (obj.previousObj != null)
                        pos.Y = obj.previousObj.bounds.Bottom;
                    else
                        pos.Y = other.Top;
                    break;
                case Alignment.FloatBottom:
                    if (obj.previousObj != null)
                        pos.Y = obj.previousObj.bounds.Top;
                    else
                        pos.Y = other.Bottom;
                    break;
            }
            return pos;
        }
        
    }
}
