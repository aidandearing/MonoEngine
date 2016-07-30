using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoEngine.UI
{
    public class UIAlignment
    {
        
        public enum Alignment { None, TopLeft, TopCenter, TopRight, CenterLeft, Center, CenterRight, BottomLeft, BottomCenter, BottomRight, Top, Left, Right, Bottom, FloatTop, FloatLeft, FloatRight, FloatBottom };
        public Alignment alignment;

        public Vector2 GetAlignment(UIObject obj, UIObject parent)
        {
            Vector2 pos = Vector2.Zero;
            switch (alignment)
            {
                case Alignment.Top:
                    pos.Y = parent.bounds.Top;
                    break;
                case Alignment.TopLeft:
                    pos.X = parent.bounds.Left;
                    pos.Y = parent.bounds.Top;
                    break;
                case Alignment.TopCenter:
                    pos.X = parent.bounds.Center.X;
                    pos.Y = parent.bounds.Top;
                    break;
                case Alignment.TopRight:
                    pos.X = parent.bounds.Right;
                    pos.Y = parent.bounds.Top;
                    break;
                case Alignment.Left:
                    pos.X = parent.bounds.Left;
                    break;
                case Alignment.CenterLeft:
                    pos.X = parent.bounds.Left;
                    pos.Y = parent.bounds.Center.Y;
                    break;
                case Alignment.Center:
                    pos.X = parent.bounds.Center.X;
                    pos.Y = parent.bounds.Center.Y;
                    break;
                case Alignment.CenterRight:
                    pos.X = parent.bounds.Right;
                    pos.Y = parent.bounds.Center.Y;
                    break;
                case Alignment.Right:
                    pos.X = parent.bounds.Right;
                    break;
                case Alignment.Bottom:
                    pos.Y = parent.bounds.Height;
                    break;
                case Alignment.BottomLeft:
                    pos.X = parent.bounds.Left;
                    pos.Y = parent.bounds.Bottom;
                    break;
                case Alignment.BottomCenter:
                    pos.X = parent.bounds.Center.X;
                    pos.Y = parent.bounds.Bottom;
                    break;
                case Alignment.BottomRight:
                    pos.X = parent.bounds.Right;
                    pos.Y = parent.bounds.Bottom;
                    break;
                case Alignment.FloatLeft:
                    if (obj.previousObj != null)
                        pos.X = obj.previousObj.bounds.Right;
                    else
                        pos.X = parent.bounds.Left;
                    break;
                case Alignment.FloatRight:
                    if (obj.previousObj != null)
                        pos.X = obj.previousObj.bounds.Left;
                    else
                        pos.X = parent.bounds.Right;
                    break;
                case Alignment.FloatTop:
                    if (obj.previousObj != null)
                        pos.Y = obj.previousObj.bounds.Bottom;
                    else
                        pos.Y = parent.bounds.Top;
                    break;
                case Alignment.FloatBottom:
                    if (obj.previousObj != null)
                        pos.Y = obj.previousObj.bounds.Top;
                    else
                        pos.Y = parent.bounds.Bottom;
                    break;
            }
            return pos;
        }
    }
}
