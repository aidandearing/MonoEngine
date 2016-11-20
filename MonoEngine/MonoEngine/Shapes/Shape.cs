using Microsoft.Xna.Framework;

namespace MonoEngine.Shapes
{
    public abstract class Shape
    {
        public const int VERTICELIMIT = 16;

        //public Transform transform;
        protected Vector3[] points = new Vector3[VERTICELIMIT];

        public Shape()
        {
            //this.transform = transform;
        }

        public Shape(Vector3[] points)
        {
            //this.transform = transform;

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

        public virtual ShapeIntersection[] Intersects(Shape shape, Transform me, Transform other)
        {
            // SAT
            return null;
        }

        public virtual bool OverlapTest(Shape shape, Transform me, Transform other)
        {
            // SAT
            return true;
        }

        public virtual Shape Overlap(Shape shape, Transform me, Transform other)
        {
            // SAT
            return null;
        }

        public virtual bool Overlap(Vector3 point, Transform me)
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
    }
}
