using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoEngine.Shapes;
using MonoEngine.Game;

namespace MonoEngine.Physics.Physics2D
{
    public class PhysicsBody2D : GameObject
    {
        public PhysicsEngine.BodyType flagBodyType = 0;
        public short flagLayer = 0;

        public Shape shape;
        private PhysicsMaterial material;

        // This list is referenced in Physic's callback registery
        internal List<Collision2D.OnCollision> collisionCallbacks;
        // This list is referenced in Physic's collision registery
        internal List<Collision2D> collisions;
        private List<Collision2D> collisions_dead;
        // This list contains a reference to any chunks the body is in, if it is static, and any chunks this body overlaps if it is dynamic
        public List<PhysicsBoundingChunk2D> chunks;

        // Physics stuff
        private Vector3 position_last;
        private Vector3 velocity;
        private Vector3 velocity_rotation;
        private Vector3 velocity_last;
        private Vector3 velocity_rotation_last;
        private Vector3 force;
        private Vector3 force_rotation;
        private Vector3 acceleration;
        private float mass;
        private float mass_i;
        private Vector3 moment_of_inertia;

        private float height;
        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;

                if (flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC) || flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC))
                {
                    // Static / Kinematic bodies are essentially infinite mass bodies, and as such, 
                    // a max float is a good way of ensuring that no other body has sufficient mass 
                    // in any physics to exert force on this one, while not immediately devolving any 
                    // physics into a giant pile of float.infinitys
                    Mass = float.MaxValue;
                }
                else
                {
                    Mass = shape.GetSurfaceArea() * height * material.Density;
                }
            }
        }

        public float Restitution
        {
            get
            {
                return material.Restitution;
            }

            set
            {
                material.Restitution = value;
            }
        }

        public float Density
        {
            get
            {
                return material.Density;
            }

            set
            {
                material.Density = value;

                if (flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC) || flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC))
                {
                    // Static / Kinematic bodies are essentially infinite mass bodies, and as such, 
                    // a max float is a good way of ensuring that no other body has sufficient mass 
                    // in any physics to exert force on this one, while not immediately devolving any 
                    // physics into a giant pile of float.infinitys
                    Mass = float.MaxValue;
                }
                else
                {
                    Mass = shape.GetSurfaceArea() * height * material.Density;
                }
            }
        }

        public float Friction
        {
            get
            {
                return material.Friction;
            }

            set
            {
                material.Friction = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return transform.parent.Position;
            }

            set
            {
                transform.parent.Position = value;
            }
        }

        public Vector3 PositionDelta
        {
            get
            {
                return Position - position_last;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }

        public Vector3 VelocityLast
        {
            get
            {
                return velocity_last;
            }
        }

        public Vector3 Acceleration
        {
            get
            {
                return acceleration;
            }
        }

        public float Mass
        {
            get
            {
                return mass;
            }

            set
            {
                mass = value;
                mass_i = 1 / mass;
            }
        }

        public PhysicsBody2D(GameObject parent, string name, Shape shape, PhysicsMaterial material, PhysicsEngine.BodyType bodyType) : base(name)
        {
            this.transform.parent = parent.transform;
            this.material = material;
            this.shape = shape;
            //this.shape.transform.parent = this.transform;
            this.flagBodyType = bodyType;

            collisionCallbacks = new List<Collision2D.OnCollision>();
            collisions = new List<Collision2D>();
            collisions_dead = new List<Collision2D>();

            chunks = new List<PhysicsBoundingChunk2D>();

            // Set up some defaults, to ensure nothing goes horribly wrong
            Height = PhysicsEngine.PhysicsSettings.DEFAULT_BODY2D_HEIGHT;

            // A body flagged as static cannot be anything but static
            if (flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC))
            {
                if (flagBodyType.HasFlag(PhysicsEngine.BodyType.RIGID))
                {
                    flagBodyType &= ~PhysicsEngine.BodyType.RIGID;
                }
                if (flagBodyType.HasFlag(PhysicsEngine.BodyType.SIMPLE))
                {
                    flagBodyType &= ~PhysicsEngine.BodyType.SIMPLE;
                }
                if (flagBodyType.HasFlag(PhysicsEngine.BodyType.KINEMATIC))
                {
                    flagBodyType &= ~PhysicsEngine.BodyType.KINEMATIC;
                }
            }

            PhysicsEngine.AddPhysicsBody(this);
        }

        public override void Update()
        {
            base.Update();

            // Static bodies need not update any of their physics, as they are immune to all forces
            if (!flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC))
            {
                velocity_last = velocity;
                position_last = transform.parent.Position;

                // If a body has been established to operate according to world forces
                if (flagBodyType.HasFlag(PhysicsEngine.BodyType.WORLDFORCE))
                {
                    force += PhysicsEngine.PhysicsSettings.WORLD_FORCE;
                }

                // Simple bodies do not have no rotational motion
                if (!flagBodyType.HasFlag(PhysicsEngine.BodyType.SIMPLE))
                {
                    // TODO Implement Rotational Motion
                }

                // Use of mass_i saves having to do the more expensive / operator, in favour of doing it once whenever mass is set/changed
                acceleration = force * mass_i;
                // TODO Implement Verlet Integration technique for motion (mabs)
                velocity = velocity + 0.5f * acceleration * Time.DeltaTime * Time.DeltaTime;
                transform.parent.Position += velocity;

                force = Vector3.Zero;
            }

            List<PhysicsBody2D> collisionTests = PhysicsEngine.GetCollisionPass(this);

            List<PhysicsBody2D> checkedAlready = new List<PhysicsBody2D>();

            foreach (PhysicsBody2D collider in collisionTests)
            {
                if (!checkedAlready.Contains(collider) && collider != this)
                {
                    Collision2D possibleCollision = Collision2D.Evaluate(this, collider);
                    if (possibleCollision != null && !collisions.Contains(possibleCollision))
                    {
                        collisions.Add(possibleCollision);
                        collider.collisions.Add(possibleCollision);
                    }

                    checkedAlready.Add(collider);
                }
            }

            foreach (Collision2D collision in collisions)
            {
                if (collision.BodyA == this)
                    collision.Resolve();
            }

            foreach (Collision2D collision in collisions_dead)
            {
                collisions.Remove(collision);
            }

            collisions_dead = new List<Collision2D>();
        }

        public void RegisterCollisionCallback(Collision2D.OnCollision callback)
        {
            PhysicsEngine.RegisterCollisionCallback(callback, this);
        }

        public void UnregisterCollisionCallback(Collision2D.OnCollision callback)
        {
            PhysicsEngine.UnregisterCollisionCallback(callback, this);
        }

        public void Remove()
        {
            PhysicsEngine.RemovePhysicsBody(this);
        }

        public void RemoveCollision(Collision2D collision)
        {
            collisions_dead.Add(collision);
        }

        /// <summary>
        /// Applies a force to the center of this body
        /// </summary>
        /// <param name="f">The force vector to apply</param>
        public void ApplyForce(Vector3 f)
        {
            force += f;
        }

        /// <summary>
        /// Applies a force to this body offset from the center by the position
        /// </summary>
        /// <param name="position">The position from the center to apply the force</param>
        /// <param name="f">The force vector to apply</param>
        public void ApplyForce(Vector3 position, Vector3 f)
        {
            // TODO ApplyForce(Vector3 position, Vector3 f)
        }

        /// <summary>
        /// Applies a force directly to the velocity of this body
        /// </summary>
        /// <param name="f">The force vector to apply</param>
        public void ApplyImpulse(Vector3 f)
        {
            velocity += f;
        }

        /// <summary>
        /// Translates the body by Vector v
        /// </summary>
        /// <param name="v">The vector to translate by</param>
        public void Translate(Vector3 v)
        {
            transform.parent.Position += v;
        }

        /// <summary>
        /// Moves the body to the position specificed
        /// </summary>
        /// <param name="position">The new position for this body</param>
        public void MoveTo(Vector3 position)
        {
            transform.parent.Position = position;
        }
    }
}
