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

        // This list is referenced in Physic's callback registery
        public List<Collision2D.OnCollision> collisionCallbacks;
        // This list is referenced in Physic's collision registery
        public List<Collision2D> collisions;
        // This list contains a reference to any chunks the body is in, if it is static, and any chunks this body overlaps if it is dynamic
        public List<PhysicsBoundingChunk2D> chunks;

        // Physics stuff
        private Vector3 velocity;
        private Vector3 velocity_rotation;
        private Vector3 velocity_last;
        private Vector3 velocity_rotation_last;
        private Vector3 force;
        private Vector3 force_rotation;
        private Vector3 acceleration;
        private float mass = PhysicsEngine.PhysicsSettings.DEFAULT_MASS;
        // ETC, later

        public PhysicsBody2D(string name, Shape shape, PhysicsEngine.BodyType bodyType) : base(name)
        {
            this.shape = shape;
            this.flagBodyType = bodyType;

            collisionCallbacks = new List<Collision2D.OnCollision>();
            collisions = new List<Collision2D>();

            chunks = new List<PhysicsBoundingChunk2D>();

            if (flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC))
            {
                mass = float.MaxValue;

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

            if (!flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC))
            {
                // TODO physics

                force += PhysicsEngine.PhysicsSettings.WORLD_FORCE;
                acceleration = force / mass;
                velocity += acceleration;

                transform.parent.Position += velocity;

                force = Vector3.Zero;
                velocity_last = velocity;
            }
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
    }
}
