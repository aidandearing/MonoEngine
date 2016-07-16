using Microsoft.Xna.Framework;
using System;

namespace MonoEngine.Physics.Physics3D
{
    internal class PhysicsEngine3D : PhysicsEngine
    {
        private Matrix worldToRender;
        private Matrix renderToWorld;

        new internal Matrix WorldToRender(Matrix matrix)
        {
            return new Matrix(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41 * worldToRender.M11, matrix.M42 * worldToRender.M22, matrix.M43 * worldToRender.M33, matrix.M44 * worldToRender.M44);
        }

        new internal Matrix RenderToWorld(Matrix matrix)
        {
            return new Matrix(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41 * renderToWorld.M11, matrix.M42 * renderToWorld.M22, matrix.M43 * renderToWorld.M33, matrix.M44 * renderToWorld.M44);
        }

        internal PhysicsEngine3D(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }

        #region GameComponent
        // GameComponent ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This region contains all the GameComponent overrides

        private Microsoft.Xna.Framework.Game game;

        /// <summary>
        /// Initialises the Physics class, this is what enables this class to be used statically
        /// </summary>
        public override void Initialize()
        {
            //registery_CollisionCallbacks = new Dictionary<PhysicsBody, List<Collision.OnCollision>>();
            //registery_ActiveCollisions = new Dictionary<PhysicsBody, List<Collision>>();

            //bodies_All = new List<PhysicsBody>();
            //bodies_Active = new List<PhysicsBody>();
            //bodies_Dead = new List<PhysicsBody>();

            worldToRender = Matrix.CreateScale(PhysicsSettings.MODEL_TRANSLATION_SCALE);
            renderToWorld = Matrix.CreateScale(1.0f / PhysicsSettings.MODEL_TRANSLATION_SCALE);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Remove all dead bodies
            //foreach (PhysicsBody body in bodies_Dead)
            //{
            //    bodies_All.Remove(body);
            //    bodies_Active.Remove(body);

            //    // If they have registered callbacks remove them
            //    if (registery_CollisionCallbacks.ContainsKey(body))
            //    {
            //        registery_CollisionCallbacks.Remove(body);
            //    }
            //}
        }
        // End of GameComponent /////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
