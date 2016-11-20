using Microsoft.Xna.Framework;
using System;
using System.Reflection;

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
            // For ease of computing around static dynamic collisions I have decided the static body should always be BodyA
            if (A.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC) || A.flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC) || B.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC) || B.flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC))
            {
                if (A.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC) || A.flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC))
                {
                    BodyA = A;
                    BodyB = B;
                }
                else
                {
                    BodyA = B;
                    BodyB = A;
                }
            }
            else
            {
                BodyA = A;
                BodyB = B;
            }

            type = PhysicsEngine.CollisionType.NONE;
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
                else
                {
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
                }

                // Evaluate the collision
                //collision.Evaluate();
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
                if ((collision.BodyB == bodyB && collision.BodyA == bodyA) || (collision.BodyA == bodyB && collision.BodyB == bodyA))
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
            bool test = BodyA.shape.OverlapTest(BodyB.shape, BodyA.transform, BodyB.transform);

            // If the test returns true, collision is occuring

            // If this type is start and test is true, collision is continuing
            if (type == PhysicsEngine.CollisionType.START && test)
            {
                type = PhysicsEngine.CollisionType.STAY;
                //BodyA.Broadcast(this, "OnCollision2DStay");
                //BodyB.Broadcast(this, "OnCollision2DStay");
                BodyA.PhysicsBody2DCallStay(this);
                BodyB.PhysicsBody2DCallStay(this);
            }
            // If this type is none and test is true, collision is starting
            if (type == PhysicsEngine.CollisionType.NONE && test)
            {
                type = PhysicsEngine.CollisionType.START;
                //BodyA.Broadcast(this, "OnCollision2DStart");
                //BodyB.Broadcast(this, "OnCollision2DStart");
                BodyA.PhysicsBody2DCallStart(this);
                BodyB.PhysicsBody2DCallStart(this);
            }
            // If this type is stop and test is false, collision is none
            if (type == PhysicsEngine.CollisionType.STOP && !test)
            {
                type = PhysicsEngine.CollisionType.NONE;
            }
            // If this type is start or stay and test is false, collision is ending
            if ((type == PhysicsEngine.CollisionType.START || type == PhysicsEngine.CollisionType.STAY) && !test)
            {
                type = PhysicsEngine.CollisionType.STOP;
                //BodyA.Broadcast(this, "OnCollision2DStop");
                //BodyB.Broadcast(this, "OnCollision2DStop");
                BodyA.PhysicsBody2DCallStop(this);
                BodyB.PhysicsBody2DCallStop(this);
            }
            // If this type is stop and test is true, collision is starting, again
            if (type == PhysicsEngine.CollisionType.STOP && test)
            {
                type = PhysicsEngine.CollisionType.START;
                //BodyA.Broadcast(this, "OnCollision2DStart");
                //BodyB.Broadcast(this, "OnCollision2DStart");
                BodyA.PhysicsBody2DCallStart(this);
                BodyB.PhysicsBody2DCallStart(this);
            }

            if (test)
            {
                foreach (Collision2D.OnCollision callback in BodyA.collisionCallbacks)
                {
                    callback.Invoke(this);
                }

                foreach (Collision2D.OnCollision callback in BodyB.collisionCallbacks)
                {
                    callback.Invoke(this);
                }
            }

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
                    if (!(BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC) || BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC)))
                    {
                        if (BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.RIGID))
                        {
                            throw new PhysicsExceptions.NotImplementedYet("Rigidbody 2D collision resolution is not yet implemented");
                        }
                        else
                        {
                            if (BodyA.shape is Shapes.Circle)
                            {
                                if (BodyB.shape is Shapes.Circle)
                                {
                                    ResolveCircleCircleSimple();
                                }
                                else if (BodyB.shape is Shapes.AABB)
                                {
                                    ResolveCircleAABBSimple();
                                }
                                else
                                {
                                    throw new PhysicsExceptions.NotImplementedYet("Poly2D collision resolution is not yet implemented");
                                }
                            }
                            else if (BodyA.shape is Shapes.AABB)
                            {
                                if (BodyB.shape is Shapes.Circle)
                                {
                                    ResolveCircleAABBSimple();
                                }
                                else if (BodyB.shape is Shapes.AABB)
                                {
                                    ResolveAABBAABBSimple();
                                }
                                else
                                {
                                    throw new PhysicsExceptions.NotImplementedYet("Poly2D collision resolution is not yet implemented");
                                }
                            }
                            else
                            {
                                throw new PhysicsExceptions.NotImplementedYet("Poly2D collision resolution is not yet implemented");
                            }
                        }
                    }
                    else if (BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC) || BodyB.flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC))
                    {
                        // Both are static

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
                            if (BodyA.shape is Shapes.Circle)
                            {
                                if (BodyB.shape is Shapes.Circle)
                                {
                                    ResolveCircleCircleStatic(BodyA, BodyB);
                                }
                                else if (BodyB.shape is Shapes.AABB)
                                {
                                    ResolveCircleAABBStatic(BodyA, BodyB);
                                }
                                else
                                {
                                    throw new PhysicsExceptions.NotImplementedYet("Poly2D collision resolution is not yet implemented");
                                }
                            }
                            else if (BodyA.shape is Shapes.AABB)
                            {
                                if (BodyB.shape is Shapes.Circle)
                                {
                                    ResolveCircleAABBStatic(BodyA, BodyB);
                                }
                                else if (BodyB.shape is Shapes.AABB)
                                {
                                    ResolveAABBAABBStatic(BodyA, BodyB);
                                }
                                else
                                {
                                    throw new PhysicsExceptions.NotImplementedYet("Poly2D collision resolution is not yet implemented");
                                }
                            }
                            else
                            {
                                throw new PhysicsExceptions.NotImplementedYet("Poly2D collision resolution is not yet implemented");
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
        }

        internal void ResolveCircleCircleSimple()
        {
            Shapes.Circle BA = BodyA.shape as Shapes.Circle;
            Shapes.Circle BB = BodyB.shape as Shapes.Circle;
            Vector3 dN = Vector3.Normalize(BA.lastOverlap_delta);
            float dR = BA.lastOverlap_radius;
            float tM = BodyA.Mass + BodyB.Mass;

            // Calculate a new center of mass for the system, and offset them both their radius away from this center of mass, along the normal, based on their mass percentage of the system
            float mA = BodyA.Mass / tM;
            float mB = BodyB.Mass / tM;
            Vector3 newCOM = BodyA.transform.Position * mA + BodyB.transform.Position * mB;
            BodyA.transform.parent.Position = newCOM - (dN * dR * (1 - mA));
            BodyB.transform.parent.Position = newCOM + (dN * dR * (1 - mB));

            // Calculate the resulting velocities based on the restitution scalars for both bodies
            if (BodyA.Velocity.LengthSquared() + BodyB.Velocity.LengthSquared() > 0)
            {
                float projection = (2 * (Vector3.Dot(BodyA.Velocity * BodyA.Restitution, dN) - Vector3.Dot(BodyB.Velocity * BodyB.Restitution, dN))) / (tM);

                BodyA.Velocity = BodyA.Velocity - projection * BodyA.Mass * dN;
                BodyB.Velocity = BodyB.Velocity + projection * BodyB.Mass * dN;
            }
        }

        // TODO fix Circle vs Circle Static collision resolution
        internal void ResolveCircleCircleStatic(PhysicsBody2D BodyS, PhysicsBody2D BodyD)
        {
            Shapes.Circle BA = BodyS.shape as Shapes.Circle;
            Shapes.Circle BB = BodyD.shape as Shapes.Circle;
            Vector3 dN = Vector3.Normalize(BA.lastOverlap_delta);
            float dR = BA.lastOverlap_radius;

            BodyD.transform.parent.Position = BodyS.transform.Position + dN * dR;

            if (BodyD.Velocity.LengthSquared() > 0)
            {
                float projection = 2 * -Vector3.Dot(BodyD.Velocity * Math.Min(BodyD.Restitution, BodyS.Restitution), dN);

                BodyD.Velocity = BodyD.Velocity + projection * dN;
            }
        }

        internal void ResolveCircleAABBSimple()
        {
            // TODO Circle AABB
            Shapes.Circle BA = (BodyA.shape is Shapes.Circle) ? BodyA.shape as Shapes.Circle : BodyB.shape as Shapes.Circle;
            Shapes.AABB BB = (BodyA.shape is Shapes.Circle) ? BodyB.shape as Shapes.AABB : BodyA.shape as Shapes.AABB;

            Vector3 dN = Vector3.Normalize(BA.lastOverlap_delta);
            float theta = (float)Math.Atan(dN.Z / dN.X);
            
            float length = BB.LengthAtAngle(theta);

            // Time to move the circle and the aabb away from each other along the normal
            // Calculate a new center of mass for the system, and offset them both their radii away from this center of mass, along the normal, based on their mass percentage of the system
            float dR = length + BA.Radius;
            float tM = BodyA.Mass + BodyB.Mass;
            float mA = BodyA.Mass / tM;
            float mB = BodyB.Mass / tM;
            Vector3 newCOM = BodyA.transform.Position * mA + BodyB.transform.Position * mB;
            BodyA.transform.parent.Position = newCOM - (dN * dR * (1 - mA));
            BodyB.transform.parent.Position = newCOM + (dN * dR * (1 - mB));

            // Calculate the resulting velocities based on the restitution scalars for both bodies
            if (BodyA.Velocity.LengthSquared() + BodyB.Velocity.LengthSquared() > 0)
            {
                float projection = (2 * (Vector3.Dot(BodyA.Velocity * BodyA.Restitution, dN) - Vector3.Dot(BodyB.Velocity * BodyB.Restitution, dN))) / (tM);

                if (BodyA.shape is Shapes.AABB)
                {
                    BodyA.Velocity = BodyA.Velocity - projection * BodyA.Mass * dN;

                    if (Math.Abs(dN.X) > Math.Abs(dN.Y))
                    {
                        BodyB.Velocity = new Vector3(-BodyB.Velocity.X, BodyB.Velocity.Y, BodyB.Velocity.Z);
                    }
                    else
                    {
                        BodyB.Velocity = new Vector3(BodyB.Velocity.X, BodyB.Velocity.Y, -BodyB.Velocity.Z);
                    }
                }
                else
                {
                    BodyB.Velocity = BodyB.Velocity + projection * BodyB.Mass * dN;

                    if (Math.Abs(dN.X) > Math.Abs(dN.Y))
                    {
                        BodyA.Velocity = new Vector3(-BodyA.Velocity.X, BodyA.Velocity.Y, BodyA.Velocity.Z);
                    }
                    else
                    {
                        BodyA.Velocity = new Vector3(BodyA.Velocity.X, BodyA.Velocity.Y, -BodyA.Velocity.Z);
                    }
                }
            }
        }

        internal void ResolveCircleAABBStatic(PhysicsBody2D BodyS, PhysicsBody2D BodyD)
        {
            // TODO Circle AABB
            Shapes.Circle BA = (BodyS.shape is Shapes.Circle) ? BodyS.shape as Shapes.Circle : BodyD.shape as Shapes.Circle;
            Shapes.AABB BB = (BodyS.shape is Shapes.Circle) ? BodyD.shape as Shapes.AABB : BodyS.shape as Shapes.AABB;

            // Might as well normalize the delta while we are at it
            Vector3 dN = Vector3.Normalize(BA.lastOverlap_delta);
            float theta = (float)Math.Atan(dN.Z / dN.X);
            float length = BB.LengthAtAngle(theta);

            // Time to move the circle and the aabb away from each other along the normal
            // Calculate a new center of mass for the system, and offset them both their radii away from this center of mass, along the normal, based on their mass percentage of the system
            float dR = length + BA.Radius;
            BodyD.transform.parent.Position = BodyS.transform.Position + (dN * dR);

            if (BodyB.Velocity.LengthSquared() > 0 && Vector3.Dot(Vector3.Normalize(BodyB.Velocity), dN) > 0)
            {
                float projection = 2 * -Vector3.Dot(BodyB.Velocity * MathHelper.Min(BodyB.Restitution, BodyA.Restitution), dN);

                if (BodyA.shape is Shapes.AABB)
                {
                    if (Math.Abs(dN.X) > Math.Abs(dN.Y))
                    {
                        BodyB.Velocity = new Vector3(-BodyB.Velocity.X, BodyB.Velocity.Y, BodyB.Velocity.Z);
                    }
                    else
                    {
                        BodyB.Velocity = new Vector3(BodyB.Velocity.X, BodyB.Velocity.Y, -BodyB.Velocity.Z);
                    }
                }
                else
                {
                    BodyB.Velocity = BodyB.Velocity + projection * BodyB.Mass * dN;
                }
            }
        }

        internal void ResolveAABBAABBSimple()
        {
            Shapes.AABB BA = BodyA.shape as Shapes.AABB;
            Shapes.AABB BB = BodyB.shape as Shapes.AABB;
            float tM = BodyA.Mass + BodyB.Mass;
            float mA = BodyA.Mass / tM;
            float mB = BodyB.Mass / tM;

            // Calculate a new center of mass for the system, and offset them both their radius away from this center of mass, along the normal
            Vector3 newCOM = BodyA.transform.Position * mA + BodyB.transform.Position * mB;

            Vector3 delta = BB.transform.Position - BA.transform.Position;
            Vector3 tD = (BA.Dimensions + BB.Dimensions) / 2;

            if (Math.Abs(delta.X) > Math.Abs(delta.Z))
            {
                if (delta.X < 0)
                {// Left
                    BodyA.transform.parent.Position = newCOM + new Vector3(tD.X * mA, 0, 0);
                    BodyB.transform.parent.Position = newCOM - new Vector3(tD.X * mB, 0, 0);
                }
                else
                {// Right
                    BodyA.transform.parent.Position = newCOM - new Vector3(tD.X * mA, 0, 0);
                    BodyB.transform.parent.Position = newCOM + new Vector3(tD.X * mB, 0, 0);
                }
            }
            else
            {
                if (delta.Z < 0)
                {// Top
                    BodyA.transform.parent.Position = newCOM + new Vector3(0, 0, tD.Z * mA);
                    BodyB.transform.parent.Position = newCOM - new Vector3(0, 0, tD.Z * mB);
                }
                else
                {// Bottom
                    BodyA.transform.parent.Position = newCOM - new Vector3(0, 0, tD.Z * mA);
                    BodyB.transform.parent.Position = newCOM + new Vector3(0, 0, tD.Z * mB);
                }
            }
        }

        internal void ResolveAABBAABBStatic(PhysicsBody2D BodyS, PhysicsBody2D BodyD)
        {
            Shapes.AABB BA = BodyS.shape as Shapes.AABB;
            Shapes.AABB BB = BodyD.shape as Shapes.AABB;

            Vector3 delta = BB.transform.Position - BA.transform.Position;
            Vector3 tD = (BA.Dimensions + BB.Dimensions) / 2;

            if (Math.Abs(delta.X) > Math.Abs(delta.Z))
            {
                if (delta.X < 0)
                {// Left
                    BodyD.transform.parent.Position = BodyS.transform.Position - new Vector3(tD.X, 0, 0);
                }
                else
                {// Right
                    BodyD.transform.parent.Position = BodyS.transform.Position + new Vector3(tD.X, 0, 0);
                }
            }
            else
            {
                if (delta.Z < 0)
                {// Top
                    BodyD.transform.parent.Position = BodyS.transform.Position - new Vector3(0, 0, tD.Z);
                }
                else
                {// Bottom
                    BodyD.transform.parent.Position = BodyS.transform.Position + new Vector3(0, 0, tD.Z);
                }
            }
        }
    }
}