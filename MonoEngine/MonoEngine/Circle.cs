using Microsoft.Xna.Framework;

namespace Capstone
{
    class Circle : Shape
    {
        public Circle(Transform transform, float radius) : base(transform)
        {
            points = new Vector3[1];
            points[0] = new Vector3(0, 0, radius);
        }

        public override ShapeIntersection[] Intersects(Shape shape)
        {
            // Circle Intersect logic

            return null;
        }

        /// <summary>
        /// Tests whether the circle is overlapping another shape
        /// </summary>
        /// <param name="shape">The shape to test overlap against</param>
        /// <returns>True if they are, false if not</returns>
        public override bool OverlapTest(Shape shape)
        {
            // Circle Intersect logic
            if (shape is Circle)
            {
                // Circle intersect Circle
                Vector3 delta = shape.transform.Position - transform.Position;

                // Check the Circle against the other shape as a circle
                if (delta.Length() <= Radius() + ((Circle)shape).Radius())
                {
                    return true;
                }
            }
            else if (shape is AABB)
            {
                // Circle intersect AABB
                Vector3 delta = shape.transform.Position - transform.Position;

                // Check the Circle against the bounding circle of the AABB
                if (delta.Length() <= Radius() + ((AABB)shape).Diagonal())
                {
                    // This means the box is fully within the circle
                    if (delta.Length() <= Radius() - ((AABB)shape).Diagonal())
                    {
                        return true;
                    }
                    else
                    {
                        // Now find out if the point on the circles edge along the normal towards the AABB is within the AABB
                        delta.Normalize();
                        delta *= points[0].Z;
                        return ((AABB)shape).Overlap(delta);
                    }
                }

                return false;
            }
            else
            {
                // Circle intersects poly
            }

            return false;
        }

        public override bool Overlap(Vector3 point)
        {
            Vector3 delta = point - transform.Position;

            if (delta.Length() <= Radius())
            {
                return true;
            }

            return false;
        }

        public override AABB GetBoundingBox()
        {
            return new AABB(transform, points[0].Z * 2, points[0].Z * 2);
        }

        public float Radius()
        {
            return points[0].Z;
        }
    }
}
