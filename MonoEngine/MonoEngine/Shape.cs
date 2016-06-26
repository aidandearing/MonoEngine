using Microsoft.Xna.Framework;

namespace Capstone
{
    abstract class Shape
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
    }
}
