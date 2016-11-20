using Microsoft.Xna.Framework;

namespace MonoEngine.Shapes
{
    public class ShapeIntersection
    {
        public Vector3 intersect;
        public Vector3 normal;
        public float depth;

        public ShapeIntersection(Vector3 intersect, Vector3 normal, float depth)
        {
            this.intersect = intersect;
            this.normal = normal;
            this.depth = depth;
        }

        // TODO ShapeIntersection needs to properly calculate the point of intersection, normal of intersection, and depth of intersection between 2 shapes
        public static ShapeIntersection Intersect(Shape shapeA, Shape shapeB)
        {
            ShapeIntersection intersection = null;

            //if (shapeA.OverlapTest(shapeB))
            //{
            //    Vector3 inter = shapeA.transform.Position - shapeB.transform.Position;
            //    Vector3 norm = Vector3.Normalize(inter);
            //    float depth = Vector3.Dot(Vector3.Normalize(shapeA.transform.Position), norm);
            //    intersection = new ShapeIntersection(inter, norm, depth);
            //}

            return intersection;
        }
    }
}
