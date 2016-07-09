using Microsoft.Xna.Framework;
using System;
using MonoEngine.Physics.Physics2D;
using MonoEngine.Physics.Physics3D;

namespace MonoEngine.Physics
{
    public class PhysicsEngine : GameComponent
    {
        public class PhysicsSettings
        {
            public static int BOUNDINGBOX_SMALLEST = 2;
            public static int BOUNDINGBOX_ORDERS = 4;
            public static int BOUNDINGBOX_LARGEST = (int)Math.Pow(BOUNDINGBOX_SMALLEST, BOUNDINGBOX_ORDERS);

            public static float MODEL_SCALE = 200.0f;

            public static Vector3 WORLD_FORCE = new Vector3(0, -9.8f, 0);
        }

        public enum EngineTypes { Physics2D = 0x00, Physics3D = 0x01 };

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

            return (engine as PhysicsEngine2D).WorldToRender(matrix);
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

            return (engine as PhysicsEngine2D).RenderToWorld(matrix);
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
