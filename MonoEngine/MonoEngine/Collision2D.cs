namespace MonoEngine
{
    namespace Physics2D
    {
        public class Collision2D
        {
            public delegate void OnCollision(Collision2D collision);

            public enum CollisionType { start, stay, stop, none };

            public PhysicsBody2D BodyA;
            public PhysicsBody2D BodyB;
            public CollisionType type;

            private Collision2D(PhysicsBody2D A, PhysicsBody2D B)
            {
                BodyA = A;
                BodyB = B;
            }

            /// <summary>
            /// Call this method to test collisions
            /// This method wants to be called as the last piece of a collision pass, after all definite non-collisions have been ruled out
            /// </summary>
            /// <param name="BodyA">The first body</param>
            /// <param name="BodyB">The second body</param>
            /// <returns>An instance of Collision</returns>
            public static Collision2D Evaluate(PhysicsBody2D bodyA, PhysicsBody2D bodyB)
            {
                // This needs to evaluate whether these 2 bodies already have a Collision between them, if so, evaluate it, and return it
                // If they don't it needs to make one, if they are colliding
                // If they aren't colliding return a new Collision with CollisionType none

                Collision2D collision;

                // Neither body has any collisions
                if (bodyA.collisions.Count == bodyB.collisions.Count && bodyA.collisions.Count == 0)
                {
                    // A new collision should be made
                    collision = new Collision2D(bodyA, bodyB);
                }

                // An optimisation here is to check the smaller list of collisions
                // Since it is an iteration
                if (bodyA.collisions.Count <= bodyB.collisions.Count)
                {
                    collision = Helper_EvaluateCollisions(bodyA, bodyB);
                }
                else
                {
                    collision = Helper_EvaluateCollisions(bodyB, bodyA);
                }

                // Evaluate the collision
                collision.Evaluate();
                // Return it
                return collision;
            }

            private static Collision2D Helper_EvaluateCollisions(PhysicsBody2D bodyA, PhysicsBody2D bodyB)
            {
                foreach (Collision2D collision in bodyA.collisions)
                {
                    if (collision.BodyB == bodyB)
                    {
                        return collision;
                    }
                }

                return new Collision2D(bodyA, bodyB);
            }

            /// <summary>
            /// Evaluates whether the collision is occuring or not
            /// </summary>
            /// <returns>Whether a collision is occuring or not</returns>
            bool Evaluate()
            {
                bool test = BodyA.shape.OverlapTest(BodyB.shape);

                // If the test returns true, collision is occuring

                // If this type is none and test is true, collision is starting
                if (type == CollisionType.none && test)
                    type = CollisionType.start;
                // If this type is start and test is true, collision is continuing
                if (type == CollisionType.start && test)
                    type = CollisionType.stay;
                // If this type is start or stay and test is false, collision is ending
                if ((type == CollisionType.start || type == CollisionType.stay) && !test)
                    type = CollisionType.stop;
                // If this type is stop and test is true, collision is starting, again
                if (type == CollisionType.stop && test)
                    type = CollisionType.start;

                return test;
            }

            void Resolve()
            {
                // TODO add collision resolution
            }
        }
    }
}