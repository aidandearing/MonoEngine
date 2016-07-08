using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoEngine.Shapes;
using MonoEngine.Game;

namespace MonoEngine.Physics2D
{
    public class PhysicsBody2D : GameObject
    {
        public enum BodyType { physics_rigidbody = 0x00, physics_static = 0x01, physics_kinematic = 0x02, physics_trigger = 0x04 };

        public BodyType flagBodyType = 0;
        public short flagLayer = 0;

        public Shape shape;

        // This list is referenced in Physic's callback registery
        public List<Collision2D.OnCollision> collisionCallbacks;
        // This list is referenced in Physic's collision registery
        public List<Collision2D> collisions;

        public PhysicsBody2D(string name, Shape shape, BodyType bodyType) : base(name)
        {
            this.shape = shape;
            this.flagBodyType = bodyType;

            collisionCallbacks = new List<Collision2D.OnCollision>();
            collisions = new List<Collision2D>();

            PhysicsEngine2D.AddPhysicsBody(this);
        }

        public override void Update()
        {
            base.Update();
            // TODO physics
        }

        public void RegisterCollisionCallback(Collision2D.OnCollision callback)
        {
            PhysicsEngine2D.RegisterCollisionCallback(callback, this);
        }

        public void UnregisterCollisionCallback(Collision2D.OnCollision callback)
        {
            PhysicsEngine2D.UnregisterCollisionCallback(callback, this);
        }

        public void Remove()
        {
            PhysicsEngine2D.RemovePhysicsBody(this);
        }
    }
}
