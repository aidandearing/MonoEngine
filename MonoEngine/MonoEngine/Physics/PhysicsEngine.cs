using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using MonoEngine.Physics.Physics2D;
using MonoEngine.Physics.Physics3D;

namespace MonoEngine.Physics
{
    public class PhysicsEngine : GameComponent
    {
        /// <summary>
        /// FLAGS: Or these together to create the specific body of your needs
        /// 
        /// SIMPLE: Has no rotational motion
        /// RIGID: Simulates real world rigidbody physics
        /// STATIC: Is unable to be moved by anything, also gets placed in the static bounding chunk system, for quick use
        /// KINEMATIC: Is unable to be stopped by anything
        /// TRIGGER: Doesn't resolve collisions
        /// WORLDFORCE: Is affected by the world force vector (gravity)
        /// INTERPOLATE: If the body has moved more than its shape dimensions on any axis, it will check across the distance traveled, instead of just where it currently is
        /// </summary>
        public enum BodyType { SIMPLE = 1, RIGID = 2, STATIC = 4, KINEMATIC = 8, TRIGGER = 16, WORLDFORCE = 32, INTERPOLATE = 64 };
        public enum CollisionType { START, STAY, STOP, NONE };

        public class PhysicsSettings
        {
            public static int BOUNDINGBOX_SMALLEST = 2;
            public static int BOUNDINGBOX_ORDERS = 4;
            public static int BOUNDINGBOX_LARGEST = (int)Math.Pow(BOUNDINGBOX_SMALLEST, BOUNDINGBOX_ORDERS);

            public static float MODEL_TRANSLATION_SCALE = 400.0f;

            public static Vector3 WORLD_FORCE = new Vector3(0, -9.8f, 0);

            public static float WORLD_DRAG = 0.1f;

            public static float DEFAULT_BODY2D_HEIGHT = 1.0f;

            public static float DEFAULT_MATERIAL_FRICTION = 0.0f;
            public static float DEFAULT_MATERIAL_RESTITUTION = 0.0f;
            public static float DEFAULT_MATERIAL_DENSITY = 1.0f;
        }

        public enum EngineTypes { Physics2D = 0x01, Physics3D = 0x02 };

        private static EngineTypes engineType;
        public static EngineTypes EngineType
        {
            get
            {
                return engineType;
            }

            private set
            {
                engineType = value;
            }
        }

        public static Matrix WorldToRender(Matrix matrix)
        {
            switch (EngineType)
            {
                case EngineTypes.Physics2D:
                    return (engine as PhysicsEngine2D).WorldToRender(matrix);
                case EngineTypes.Physics3D:
                    return (engine as PhysicsEngine3D).WorldToRender(matrix);
            }

            throw new PhysicsExceptions.UnsupportedEngine("The Physics Engine must either be set to EngineTypes.Physics2D, or EngineTypes.Physics3D");
        }

        public static Matrix RenderToWorld(Matrix matrix)
        {
            switch (EngineType)
            {
                case EngineTypes.Physics2D:
                    return (engine as PhysicsEngine2D).RenderToWorld(matrix);
                case EngineTypes.Physics3D:
                    return (engine as PhysicsEngine3D).RenderToWorld(matrix);
            }

            throw new PhysicsExceptions.UnsupportedEngine("The Physics Engine must either be set to EngineTypes.Physics2D, or EngineTypes.Physics3D");
        }

        // TODO 3D overload of this (PhysicsBody3D body)
        public static void AddPhysicsBody(PhysicsBody2D body)
        {
            switch (EngineType)
            {
                case EngineTypes.Physics2D:
                    (engine as PhysicsEngine2D).AddBody(body);
                    break;
                case EngineTypes.Physics3D:
                    throw new PhysicsExceptions.InvalidPhysicsBody("Cannot add a PhysicsBody2D to a PhysicsEngine3D");
            }
        }

        // TODO 3D overload of this (PhysicsBody3D body)
        public static void RemovePhysicsBody(PhysicsBody2D body)
        {
            switch (EngineType)
            {
                case EngineTypes.Physics2D:
                    (engine as PhysicsEngine2D).RemoveBody(body);
                    break;
                case EngineTypes.Physics3D:
                    throw new PhysicsExceptions.InvalidPhysicsBody("Cannot remove a PhysicsBody2D from a PhysicsEngine3D");
            }
        }

        // TODO 3D overload of this (Collision3D.OnCollision callback, PhysicsBody3D body)
        public static void RegisterCollisionCallback(Collision2D.OnCollision callback, PhysicsBody2D body)
        {
            switch (EngineType)
            {
                case EngineTypes.Physics2D:
                    (engine as PhysicsEngine2D).RegisterCollisionCallback(callback, body);
                    break;
                case EngineTypes.Physics3D:
                    throw new PhysicsExceptions.InvalidCollisionCallback("Cannot register a Collision2D.OnCollision callback to a PhysicsBody2D in a PhysicsEngine3D");
            }
        }

        // TODO 3D overload of this (Collision3D.OnCollision callback, PhysicsBody3D body)
        public static void UnregisterCollisionCallback(Collision2D.OnCollision callback, PhysicsBody2D body)
        {
            switch (EngineType)
            {
                case EngineTypes.Physics2D:
                    (engine as PhysicsEngine2D).UnregisterCollisionCallback(callback, body);
                    break;
                case EngineTypes.Physics3D:
                    throw new PhysicsExceptions.InvalidCollisionCallback("Cannot unregister a Collision2D.OnCollision callback from a PhysicsBody2D in a PhysicsEngine3D");
            }
        }

        // TODO 3D overload of this (PhysicsBody3D body)
        public static List<PhysicsBody2D> GetCollisionPass(PhysicsBody2D body)
        {
            switch(EngineType)
            {
                case EngineTypes.Physics2D:
                    return (engine as PhysicsEngine2D).GetCollisionPass(body);
                case EngineTypes.Physics3D:
                    throw new PhysicsExceptions.InvalidPhysicsBody("Cannot test for possible collisions on a PhysicsBody2D in a PhysicsEngine3D");
            }

            throw new PhysicsExceptions.UnsupportedEngine("Cannot test for possible collisions on a PhysicsBody2D in a " + EngineType.ToString());
        }

        // TODO 3D overload of this (PhysicsBody3D body)
        public static List<int> CalculateBoundsIndices(PhysicsBody2D body)
        {
            switch (EngineType)
            {
                case EngineTypes.Physics2D:
                    return (engine as PhysicsEngine2D).CalculateBoundsIndices(body);
                case EngineTypes.Physics3D:
                    throw new PhysicsExceptions.InvalidPhysicsBody("Cannot test for possible collisions on a PhysicsBody2D in a PhysicsEngine3D");
            }

            throw new PhysicsExceptions.UnsupportedEngine("Cannot test for possible collisions on a PhysicsBody2D in a " + EngineType.ToString());
        }

        private static PhysicsEngine engine;

        internal PhysicsEngine(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }

        /// <summary>
        /// Instances the physics engine as either 2D or 3D
        /// </summary>
        /// <param name="game">The Game instance running</param>
        /// <returns>The Physics instance running</returns>
        public static PhysicsEngine Instance(Microsoft.Xna.Framework.Game game, EngineTypes type)
        {
            switch (type)
            {
                case EngineTypes.Physics2D:
                    engine = (engine == null) ? new Physics2D.PhysicsEngine2D(game) : engine;
                    break;
                case EngineTypes.Physics3D:
                    engine = (engine == null) ? new Physics3D.PhysicsEngine3D(game) : engine;
                    break;
                default:
                    engine = (engine == null) ? new Physics2D.PhysicsEngine2D(game) : engine;
                    break;
            }

            EngineType = type;

            return engine;
        }
    }
}
