using Microsoft.Xna.Framework;
using System;

namespace MonoEngine.Shapes
{
    [Serializable]
    public class Circle : Shape
    {
        public Vector3 lastOverlap_delta;
        public float lastOverlap_radius;

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
                // Circle overlap Circle
                Vector3 delta =  transform.Position- shape.transform.Position;
                lastOverlap_delta = delta;
                lastOverlap_radius = Radius + ((Circle)shape).Radius;

                // Check the Circle against the other shape as a circle
                if (delta.Length() < lastOverlap_radius)
                {
                    return true;
                }
            }
            else if (shape is AABB)
            {
                // Circle intersect AABB
                Vector3 delta = transform.Position - shape.transform.Position;
                lastOverlap_delta = delta;
                //Vector3 dN = Vector3.Normalize(delta);

                if (delta.Length() <= Radius + (shape as AABB).Diagonal)
                {
                    // The Circle and the AABB bounding circle are overlapping
                    if (this.GetBoundingBox().OverlapTest(shape))
                    {
                        return true;
                    }
                }

                //// Circle intersect AABB
                //Vector3 delta = shape.transform.Position - transform.Position;
                //lastOverlap_delta = delta;
                //Vector3 dN = Vector3.Normalize(delta);
                //float theta = (float)Math.Atan(dN.Z / dN.X);
                //float length = ((AABB)shape).LengthAtAngle(theta);

                //// Check the Circle against the bounding circle of the AABB
                //if (delta.Length() < Radius + length)
                //{
                //    // This means the box is fully within the circle
                //    if (delta.Length() <= Radius - length)
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        // Now find out if the point on the circles edge along the normal towards the AABB is within the AABB
                //        delta.Normalize();
                //        delta *= points[0].Z;
                //        return ((AABB)shape).Overlap(delta);
                //    }
                //}

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

            if (delta.Length() <= Radius)
            {
                return true;
            }

            return false;
        }

        public override AABB GetBoundingBox()
        {
            return new AABB(transform, points[0].Z * 2, points[0].Z * 2);
        }

        public float Radius
        {
            get
            {
                return points[0].Z;
            }

            set
            {
                points[0].Z = value;
            }
        }

        public override float GetSurfaceArea()
        {
            return 2 * MathHelper.Pi * Radius;
        }
    }
}
