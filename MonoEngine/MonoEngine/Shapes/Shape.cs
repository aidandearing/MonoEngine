using Microsoft.Xna.Framework;
using System;
using System.Xml;
using System.Collections.Generic;

namespace MonoEngine.Shapes
{
    [Serializable]
    public abstract class Shape
    {
        public const int VERTICELIMIT = 16;

        public Transform transform;
        protected Vector3[] points = new Vector3[VERTICELIMIT];

        public Shape(Transform transform)
        {
            this.transform = transform;
        }

        public Shape(Transform transform, Vector3[] points)
        {
            this.transform = transform;

            if (points.Length <= VERTICELIMIT)
            {
                this.points = points;
            }
            else
            {
                for (int i = 0; i < VERTICELIMIT; ++i)
                {
                    this.points[i] = points[i];
                }
            }
        }

        public virtual ShapeIntersection[] Intersects(Shape shape)
        {
            // SAT
            return null;
        }

        public virtual bool OverlapTest(Shape shape)
        {
            // SAT
            return true;
        }

        public virtual Shape Overlap(Shape shape)
        {
            // SAT
            return null;
        }

        public virtual bool Overlap(Vector3 point)
        {
            // ???
            return true;
        }

        public virtual AABB GetBoundingBox()
        {
            // Get min and max values of cloud of points, build AABB that encompasses said points
            return null;
        }

        public virtual float GetSurfaceArea()
        {
            // Calculate surface area by converting every vertex into a triangle with the next one and the center, then calculate the surface area of each of those and sum them
            return 0;
        }

        //////////////////////////////////////////////////////////////////////////////
        // CASTS
        //////////////////////////////////////////////////////////////////////////////

        public static Shape XMLToShape(XmlReader reader)
        {
            Shape shape = null;

            int depth = reader.Depth;
            int depth_inner_1;

            int currentVector = -1;

            List<Vector3> vs = new List<Vector3>();

            while (reader.Read() && depth < reader.Depth)
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "vector3":
                            currentVector++;
                            depth_inner_1 = reader.Depth;

                            Vector3 v = new Vector3();

                            while (reader.Read() && depth_inner_1 < reader.Depth)
                            {
                                if (reader.IsStartElement())
                                {
                                    switch (reader.Name)
                                    {
                                        case "x":
                                            reader.Read();
                                            v.X = float.Parse(reader.Value);
                                            break;
                                        case "y":
                                            reader.Read();
                                            v.Y = float.Parse(reader.Value);
                                            break;
                                        case "z":
                                            reader.Read();
                                            v.Z = float.Parse(reader.Value);
                                            break;
                                    }
                                }
                            }
                            vs.Add(v);
                            break;
                    }
                }
            }

            return shape;
        }

        public override string ToString()
        {
            string self = "";

            foreach (Vector3 point in points)
            {
                self += "\n\t<Vector3>\n" +
                "\t\t<x>" + point.X + "</x>\n" +
                "\t\t<y>" + point.Y + "</y>\n" +
                "\t\t<z>" + point.Z + "</z>\n" +
                "\t</Vector3>\n";
            }

            return self;
        }
    }
}
