using Microsoft.Xna.Framework;
using System;

namespace MonoEngine.Shapes
{
    public class PhysicsAABB : Shape
    {
        public Transform transform;
        private float diagonal;
        private Vector3 dimensions;

        public PhysicsAABB(Transform transform, float width, float height) : base()
        {
            this.transform = transform;
            float halfwidth = width / 2;
            float halfheight = height / 2;

            points = new Vector3[2];
            points[0] = new Vector3(-halfwidth, 0, -halfheight);
            points[1] = new Vector3(halfwidth, 0, halfheight);

            dimensions = new Vector3(width, 0, height);
            diagonal = points[0].Length();
        }

        public override ShapeIntersection[] Intersects(Shape shape, Transform me, Transform other)
        {
            // AABB Intersect logic

            return null;
        }

        public override bool OverlapTest(Shape shape, Transform me, Transform other)
        {
            // AABB Intersect logic
            if (shape is Circle)
            {
                // The Circle already knows how to check against an AABB, why code it again?
                return shape.OverlapTest(this, other, me);
            }
            else if (shape is AABB)
            {
                // Do I want to do a quick bounding circle test before doing an AABB test?
                // Is it worth it.

                //  Cast the shape to an AABB
                AABB shapeAsAABB = shape as AABB;

                // If my right is less than its left I am off its left side
                if (Right(me).X < shapeAsAABB.Left(other).X)
                    return false;
                // If my left is greater than its right I am off its right side
                if (Left(me).X > shapeAsAABB.Right(other).X)
                    return false;
                // If my down is less than its up I am off its top
                if (Down(me).Z < shapeAsAABB.Up(other).Z)
                    return false;
                // If my up is greater than its down I am off its bottom
                if (Up(me).Z > shapeAsAABB.Down(other).Z)
                    return false;
            }
            else
            {
                // ???
            }

            return true;
        }

        public override bool Overlap(Vector3 point, Transform me)
        {
            // AABB Intersect logic
            // If the point is left of the left, no intersect
            if (point.X < points[0].X + me.Position.X)
                return false;
            // If the point is right of the right, no intersect
            if (point.X > points[1].X + me.Position.X)
                return false;
            // If the point is beneath the bottom, no intersect
            if (point.Z < points[0].Z + me.Position.Z)
                return false;
            // If the point is above the top, no intersect
            if (point.Z > points[1].Z + me.Position.Z)
                return false;

            return true;
        }

        public override AABB GetBoundingBox()
        {
            return new AABB(dimensions.X, dimensions.Z);
        }

        public Vector3 Left(Transform me)
        {
            return me.Position + new Vector3(points[0].X, 0, 0);
        }

        public Vector3 Up(Transform me)
        {
            return me.Position + new Vector3(0, 0, points[0].Z);
        }

        public Vector3 Right(Transform me)
        {
            return me.Position + new Vector3(points[1].X, 0, 0);
        }

        public Vector3 Down(Transform me)
        {
            return me.Position + new Vector3(0, 0, points[1].Z);
        }

        public Vector3 Width()
        {
            return new Vector3(dimensions.X, 0, 0);
        }

        public Vector3 Height()
        {
            return new Vector3(0, 0, dimensions.Z);
        }

        public Vector3 Min(Transform me)
        {
            return me.Position + points[0];
        }

        public Vector3 Max(Transform me)
        {
            return me.Position + points[1];
        }

        public float Diagonal
        {
            get
            {
                return diagonal;
            }
        }

        public Vector3 Dimensions
        {
            get
            {
                return dimensions;
            }
            set
            {
                dimensions = value;

                float halfwidth = dimensions.X / 2;
                float halfheight = dimensions.Z / 2;

                points[0].X = -halfwidth;
                points[0].Z = -halfheight;
                points[1].X = halfwidth;
                points[1].Z = halfheight;

                diagonal = points[0].Length();
            }
        }

        public float LengthAtAngle(float theta)
        {
            // -------x----------------
            // |       \              |
            // |        \             |
            // |         \.           |
            // |                      |
            // |                      |
            // ------------------------
            // Given an angle from the center of an AABB determine the length from the center to the edge of the AABB on that angle

            // Next lets figure out what quad the angle is in
            // This is important because it changes the knowns for the equation
            int quad = (int)Math.Floor(((theta + MathHelper.PiOver4 / 2) % MathHelper.TwoPi) / MathHelper.PiOver2);

            float knownDim = 0;

            if (quad == 0 || quad == 2)
            {
                // Quad 1 & Quad 3 (they have the same knowns)
                // In Quads 1 & 3 the known is half the width
                knownDim = Dimensions.X;
            }
            else if (quad == 1 || quad == 3)
            {
                // Quad 2 & Quad 4 (they have the same knowns)
                // In Quads 2 & 4 the known is half the height
                knownDim = Dimensions.Z;
            }

            // Next lets isolate the angle to the quad we are in
            theta = theta % MathHelper.PiOver2;

            // Now lets use this angle and the known dimension to calculate the length to the edge
            return (knownDim / 2) / (float)Math.Cos(theta);
        }

        public override float GetSurfaceArea()
        {
            return dimensions.X * dimensions.Z;
        }
    }
}
