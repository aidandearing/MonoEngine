using Microsoft.Xna.Framework;

namespace MonoEngine.Physics.Physics2D
{
    public class Collision2D
    {
        public delegate void OnCollision(Collision2D collision);

        public PhysicsBody2D BodyA;
        public PhysicsBody2D BodyB;
        public PhysicsEngine.CollisionType type;

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
            // No need to check collisions on a body with itself
            if (bodyA != bodyB)
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
            else
                return null;
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
        public bool Evaluate()
        {
            bool test = BodyA.shape.OverlapTest(BodyB.shape);

            // If the test returns true, collision is occuring

            // If this type is none and test is true, collision is starting
            if (type == PhysicsEngine.CollisionType.NONE && test)
                type = PhysicsEngine.CollisionType.START;
            // If this type is start and test is true, collision is continuing
            if (type == PhysicsEngine.CollisionType.START && test)
                type = PhysicsEngine.CollisionType.STAY;
            // If this type is stop and test is false, collision is none
            if (type == PhysicsEngine.CollisionType.STOP && !test)
                type = PhysicsEngine.CollisionType.NONE;
            // If this type is start or stay and test is false, collision is ending
            if ((type == PhysicsEngine.CollisionType.START || type == PhysicsEngine.CollisionType.STAY) && !test)
                type = PhysicsEngine.CollisionType.STOP;
            // If this type is stop and test is true, collision is starting, again
            if (type == PhysicsEngine.CollisionType.STOP && test)
                type = PhysicsEngine.CollisionType.START;

            return test;
        }

        public void Resolve()
        {
            // TODO add collision resolution
            if (Evaluate())
            {
                // If a body is static, and another is kinematic, or both are static, or both are kinematic, resolution of a collision is not
                if (!((BodyA.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC) || BodyA.flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC)) && (BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC) || BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC))))
                {
                    // The bodies are not all kinematic or static (1 could be either static or kinematic, but not both)
                    if (BodyA.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC) || BodyA.flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC))
                    {
                        // BodyA is static or kinematic and therefore should not move
                    }
                    else if (BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC) || BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC))
                    {
                        // BodyB is static or kinematic and therefore should not move
                    }
                    else
                    {
                        // Neither body is static or kinematic, and therefore should both move
                        if (BodyA.flagBodyType.HasFlag(PhysicsEngine.BodyType.RIGID) || BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.RIGID))
                        {
                            throw new PhysicsExceptions.NotImplementedYet("Rigidbody 2D collision resolution is not yet implemented");
                        }
                        else
                        {
                            // Both bodies are PhysicsEngine.BodyType.SIMPLE
                            // Lets resolve BodyA first
                            if (BodyA.shape is Shapes.Circle)
                            {
                                if (BodyB.shape is Shapes.Circle)
                                {
                                    // TODO Circle Circle
                                    Shapes.Circle BA = BodyA.shape as Shapes.Circle;
                                    Vector3 dN = BA.lastOverlap_delta / BA.lastOverlap_radius;
                                    float tM = BodyA.Mass + BodyB.Mass;

                                    // I need to apply a force to BodyA along the inverse normal that is proportional to its mass
                                    BodyA.ApplyForce(Vector3.Negate(dN) * (1 - (BodyA.Mass / tM)) * BA.lastOverlap_radius);
                                    BodyB.ApplyForce(dN * (1 - (BodyB.Mass / tM)) * BA.lastOverlap_radius);
                                }
                                else if (BodyB.shape is Shapes.AABB)
                                {
                                    // TOD Circle AABB
                                }
                                else
                                {
                                    throw new PhysicsExceptions.NotImplementedYet("Non Circle/AABB 2D collision resolution is not yet implemented");
                                }
                            }
                            else if (BodyA.shape is Shapes.AABB)
                            {
                                if (BodyB.shape is Shapes.Circle)
                                {
                                    // TODO AABB Circle
                                }
                                else if (BodyB.shape is Shapes.AABB)
                                {
                                    // TODO AABB AABB
                                }
                                else
                                {
                                    throw new PhysicsExceptions.NotImplementedYet("Non Circle/AABB 2D collision resolution is not yet implemented");
                                }
                            }
                            else
                            {
                                throw new PhysicsExceptions.NotImplementedYet("Non Circle/AABB 2D collision resolution is not yet implemented");
                            }
                        }
                    }
                }

            }
            else
            {
                if (type == PhysicsEngine.CollisionType.NONE)
                {
                    BodyA.RemoveCollision(this);
                    BodyB.RemoveCollision(this);
                }
            }

            foreach (Collision2D.OnCollision callback in BodyA.collisionCallbacks)
            {
                callback.Invoke(this);
            }

            foreach (Collision2D.OnCollision callback in BodyB.collisionCallbacks)
            {
                callback.Invoke(this);
            }
        }
    }
}