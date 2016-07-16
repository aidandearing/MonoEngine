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
    class UIObject
    {
        public enum Alignment { None, TopLeft, TopCenter, TopRight, Top, Left, CenterLeft, Center, CenterRight, Right, BottomLeft, BottomCenter, BottomRight, Bottom };

        public Dictionary<string, UIObject> UIObjects;

        public UIScreen Screen { get; set; }
        public UIObject Parent { get; set; }
        public Rectangle Bounds { get; set; }
        public Alignment Align { get; set; }
        public Alignment BoundsAlign { get; set; }
        public Vector2 Origin { get; set; }
        //what is depth?
        public float Depth { get; set; }

        protected List<string> ComponentRefs;
        protected List<UIObject> Components;
        protected List<UIObject> Disposables;
        protected List<UIBehaviour> Behaviours;
        protected List<UIBehaviour> DeadBehaviours;


        public UIObject()
        {
            Components = new List<UIObject>();
            Disposables = new List<UIObject>();
            Behaviours = new List<UIBehaviour>();
            DeadBehaviours = new List<UIBehaviour>();


            Depth = 0;
        }
        public virtual void Update()
        {
            foreach (UIObject obj in Components)
            {
                obj.Update();
            }

            foreach (UIBehaviour b in Behaviours)
            {
                b.Update();
            }

            foreach (UIObject obj in Disposables)
            {
                Components.Remove(obj);
            }

            foreach (UIBehaviour b in DeadBehaviours)
            {
                Behaviours.Remove(b);
            }

            Disposables = new List<UIObject>();
            DeadBehaviours = new List<UIBehaviour>();

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (UIObject obj in Components)
            {
                obj.Draw(spriteBatch);
            }
        }
        public void AddObject(UIObject obj)
        {
            Components.Add(obj);
        }
        public void RemoveObject(UIObject obj)
        {
            Disposables.Add(obj);
        }
        public void AddBehaviour(UIBehaviour b)
        {
            Behaviours.Add(b);
        }
        public void RemoveBehaviour(UIBehaviour b)
        {
            DeadBehaviours.Add(b);
        }
        //whats this do exactly?
        public void End()
        {
            if (Parent != null)
                Parent.RemoveObject(this);
            //else
                //Screen.RemoveObject(this);
        }
        public UIObject Clone()
        {
            UIObject obj = (UIObject)MemberwiseClone();

            return obj;
        }
        public void SetAlignment(Alignment alignment)
        {
            Rectangle parentBounds = new Rectangle();

            if (Parent == null)
                parentBounds = Parent.Bounds;
            else
                parentBounds = new Rectangle(0, 0, (int)Render.GraphicsHelper.screen.Size.X, (int)Render.GraphicsHelper.screen.Size.Y);

            Point pos = Bounds.Location;
            Point size = Bounds.Size;

            switch (alignment)
            {
                case Alignment.TopLeft:
                    pos = parentBounds.Location;
                    break;
                case Alignment.TopCenter:
                    pos = new Point(parentBounds.Center.X, parentBounds.Y);
                    break;
                case Alignment.TopRight:
                    pos = new Point(parentBounds.Right - size.X, parentBounds.Y);
                    break;
                case Alignment.Top:
                    pos = new Point(pos.X, parentBounds.Y);
                    break;
                case Alignment.Left:
                    pos = new Point(parentBounds.X, pos.Y);
                    break;
                case Alignment.CenterLeft:
                    pos = new Point(parentBounds.Left, parentBounds.Center.Y);
                    break;
                case Alignment.Center:
                    pos = parentBounds.Center;
                    break;
                case Alignment.CenterRight:
                    pos = new Point(parentBounds.Right - size.X, parentBounds.Center.Y);
                    break;
                case Alignment.Right:
                    pos = new Point(parentBounds.Right - size.X, pos.Y);
                    break;
                case Alignment.BottomLeft:
                    pos = new Point(parentBounds.Left, parentBounds.Bottom - size.Y);
                    break;
                case Alignment.BottomCenter:
                    pos = new Point(parentBounds.Center.X, parentBounds.Bottom - size.Y);
                    break;
                case Alignment.BottomRight:
                    pos = new Point(parentBounds.Right - size.X, parentBounds.Bottom - size.Y);
                    break;
                case Alignment.Bottom:
                    pos = new Point(pos.X, parentBounds.Bottom - size.Y);
                    break;
            }
            Bounds = new Rectangle(pos, size);
        }
        public Rectangle GetAlignment(Alignment alignment)
        {
            Rectangle parentBounds = new Rectangle();
            Rectangle bounds = Bounds;

            if (Parent == null)
                parentBounds = Parent.Bounds;
            else
                parentBounds = new Rectangle(0, 0, (int)Render.GraphicsHelper.screen.Width, (int)Render.GraphicsHelper.screen.Height);

            Point pos = Bounds.Location;
            Point size = Bounds.Size;

            switch (alignment)
            {
                case Alignment.TopLeft:
                    pos = parentBounds.Location;
                    break;
                case Alignment.TopCenter:
                    pos = new Point(parentBounds.Center.X, parentBounds.Y);
                    break;
                case Alignment.TopRight:
                    pos = new Point(parentBounds.Right - size.X, parentBounds.Y);
                    break;
                case Alignment.Top:
                    pos = new Point(pos.X, parentBounds.Y);
                    break;
                case Alignment.Left:
                    pos = new Point(parentBounds.X, pos.Y);
                    break;
                case Alignment.CenterLeft:
                    pos = new Point(parentBounds.Left, parentBounds.Center.Y);
                    break;
                case Alignment.Center:
                    pos = parentBounds.Center;
                    break;
                case Alignment.CenterRight:
                    pos = new Point(parentBounds.Right - size.X, parentBounds.Center.Y);
                    break;
                case Alignment.Right:
                    pos = new Point(parentBounds.Right - size.X, pos.Y);
                    break;
                case Alignment.BottomLeft:
                    pos = new Point(parentBounds.Left, parentBounds.Bottom - size.Y);
                    break;
                case Alignment.BottomCenter:
                    pos = new Point(parentBounds.Center.X, parentBounds.Bottom - size.Y);
                    break;
                case Alignment.BottomRight:
                    pos = new Point(parentBounds.Right - size.X, parentBounds.Bottom - size.Y);
                    break;
                case Alignment.Bottom:
                    pos = new Point(pos.X, parentBounds.Bottom - size.Y);
                    break;
            }
            return Bounds = new Rectangle(pos, size);
        }
        public void SetOrigin(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.TopLeft:
                    Origin = new Vector2(0.0f, 0.0f);
                    break;
                case Alignment.TopCenter:
                    Origin = new Vector2(Bounds.Center.X, 0.0f);
                    break;
                case Alignment.TopRight:
                    Origin = new Vector2(Bounds.Width, 0.0f);
                    break;
                case Alignment.Top:
                    Origin = new Vector2(Bounds.X, 0.0f);
                    break;
                case Alignment.Left:
                    Origin = new Vector2(0.0f, Bounds.Y);
                    break;
                case Alignment.CenterLeft:
                    Origin = new Vector2(0.0f, Bounds.Center.Y);
                    break;
                case Alignment.Center:
                    Origin = new Vector2(Bounds.Center.X, Bounds.Center.Y);
                    break;
                case Alignment.CenterRight:
                    Origin = new Vector2(Bounds.Width, Bounds.Center.Y);
                    break;
                case Alignment.Right:
                    Origin = new Vector2(Bounds.Width, Bounds.Y);
                    break;
                case Alignment.BottomLeft:
                    Origin = new Vector2(0.0f, Bounds.Height);
                    break;
                case Alignment.BottomCenter:
                    Origin = new Vector2(Bounds.Center.X, Bounds.Height);
                    break;
                case Alignment.BottomRight:
                    Origin = new Vector2(Bounds.Width, Bounds.Height);
                    break;
                case Alignment.Bottom:
                    Origin = new Vector2(Bounds.X, Bounds.Height);
                    break;

            }
        }
        public Vector2 GetOrigin(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.TopLeft:
                    return new Vector2(0.0f, 0.0f);
                case Alignment.TopCenter:
                    return new Vector2(Bounds.Center.X, 0.0f);
                case Alignment.TopRight:
                    return new Vector2(Bounds.Width, 0.0f);
                case Alignment.Top:
                    return new Vector2(Bounds.X, 0.0f);
                case Alignment.Left:
                    return new Vector2(0.0f, Bounds.Y);
                case Alignment.CenterLeft:
                    return new Vector2(0.0f, Bounds.Center.Y);
                case Alignment.Center:
                    return new Vector2(Bounds.Center.X, Bounds.Center.Y);
                case Alignment.CenterRight:
                    return new Vector2(Bounds.Width, Bounds.Center.Y);
                case Alignment.Right:
                    return new Vector2(Bounds.Width, Bounds.Y);
                case Alignment.BottomLeft:
                    return new Vector2(0.0f, Bounds.Height);
                case Alignment.BottomCenter:
                    return new Vector2(Bounds.Center.X, Bounds.Height);
                case Alignment.BottomRight:
                    return new Vector2(Bounds.Width, Bounds.Height);
                case Alignment.Bottom:
                    return new Vector2(Bounds.X, Bounds.Height);
                default:
                    return Vector2.Zero;
            }
        }
        public static Rectangle ReaderToBounds(UIObject parent, XmlReader reader)
        {
            Rectangle bounds = new Rectangle();

            int depth = reader.Depth;

            while (reader.Read() && reader.Depth > depth)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "x":
                            //check if the value is a float be searching for a "."
                            if (reader.Read() && reader.Value.Contains("."))
                                bounds = new Rectangle((int)(float.Parse(reader.Value) * Render.GraphicsHelper.screen.Width), bounds.Y, bounds.Width, bounds.Height);
                            else
                                //else the value is an integar
                                bounds = new Rectangle((int.Parse(reader.Value) * Render.GraphicsHelper.screen.Width), bounds.Y, bounds.Width, bounds.Height);
                            break;
                        case "y":
                            if (reader.Read() && reader.Value.Contains("."))
                                bounds = new Rectangle(bounds.X, (int)(float.Parse(reader.Value) * Render.GraphicsHelper.screen.Height), bounds.Width, bounds.Height);
                            else
                                bounds = new Rectangle(bounds.X, (int.Parse(reader.Value) * Render.GraphicsHelper.screen.Height), bounds.Width, bounds.Height);
                            break;
                        case "w":
                            if (reader.Read() && reader.Value.Contains("."))
                                bounds = new Rectangle(bounds.X, bounds.Y, (int)(float.Parse(reader.Value) * Render.GraphicsHelper.screen.Width), bounds.Height);
                            else
                                bounds = new Rectangle(bounds.X, bounds.Y, (int.Parse(reader.Value) * Render.GraphicsHelper.screen.Width), bounds.Height);
                            break;
                        case "h":
                            if (reader.Read() && reader.Value.Contains("."))
                                bounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, (int)(float.Parse(reader.Value) * Render.GraphicsHelper.screen.Height));
                            else
                                bounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, (int.Parse(reader.Value) * Render.GraphicsHelper.screen.Height));
                            break;
                    }
                }
            }
            parent.SetOrigin(parent.BoundsAlign);

            return bounds;
        }
        public static List<UIBehaviour> ReaderToBehaviours(UIObject parent, XmlReader reader)
        {
            List<UIBehaviour> behaviours = new List<UIBehaviour>();

            int depth = reader.Depth;

            while (reader.Read() && reader.Depth > depth)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "behaviour":
                            int depth2 = reader.Depth;
                            while (reader.Read() && reader.Depth > depth2)
                            {
                                if (reader.IsStartElement())
                                {
                                    switch (reader.Name)
                                    {
                                        case "goal":
                                            break;
                                        case "trigger":
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            return behaviours;
        }
        public static List<string> ReaderToObjectRefs(UIObject parent, XmlReader reader)
        {
            List<string> refs = new List<string>();

            int depth = reader.Depth;

            while (reader.Read() && reader.Depth > depth)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "ref":
                            if (reader["type"] == "object" && reader.Read())
                                refs.Add(reader.Value);
                            break;
                    }
                }
            }
            return refs;
        }
        //what does this do exactly?
        public static UIObject PathToObject(string path)
        {
            UIObject obj = new UIObject();

            string type = "container";

            using (XmlReader reader = XmlReader.Create(path))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "container":
                                type = reader.Name;
                                obj = new UIContainer(reader);
                                break;
                            case "button":
                                type = reader.Name;
                                obj = new UIButton(reader);
                                break;
                            case "slider":
                                type = reader.Name;
                                obj = new UISlider(reader);
                                break;
                            case "image":
                                type = reader.Name;
                                obj = new UIImage(reader);
                                break;
                            case "text":
                                type = reader.Name;
                                obj = new UIText(reader);
                                break;
                        }
                    }
                }
            }
            return obj;
        }

    }
}