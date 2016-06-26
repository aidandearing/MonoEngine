﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoEngine.Shapes;
using MonoEngine.Game;

namespace MonoEngine
{
    namespace Physics2D
    {
        public class PhysicsBody : GameObject
        {
            public enum BodyType { physics_rigidbody = 0x00, physics_static = 0x01, physics_kinematic = 0x02, physics_trigger = 0x04 };

            public BodyType flagBodyType = 0;
            public short flagLayer = 0;

            public Shape shape;

            // This list is referenced in Physic's callback registery
            public List<Collision.OnCollision> collisionCallbacks;
            // This list is referenced in Physic's collision registery
            public List<Collision> collisions;

            public PhysicsBody(string name, Shape shape, BodyType bodyType) : base(name)
            {
                this.shape = shape;
                this.flagBodyType = bodyType;

                collisionCallbacks = new List<Collision.OnCollision>();
                collisions = new List<Collision>();

                PhysicsEngine.AddPhysicsBody(this);
            }

            public override void Update()
            {
                base.Update();
                // TODO physics
            }

            public void RegisterCollisionCallback(Collision.OnCollision callback)
            {
                PhysicsEngine.RegisterCollisionCallback(callback, this);
            }

            public void UnregisterCollisionCallback(Collision.OnCollision callback)
            {
                PhysicsEngine.UnregisterCollisionCallback(callback, this);
            }

            public void Remove()
            {
                PhysicsEngine.RemovePhysicsBody(this);
            }
        }
    }
}
