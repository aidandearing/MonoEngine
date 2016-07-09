using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using MonoEngine.Shapes;

namespace MonoEngine.Physics.Physics2D
{
    internal class PhysicsEngine2D : PhysicsEngine
    {
        private Matrix worldToRender;
        private Matrix renderToWorld;

        internal Matrix WorldToRender(Matrix matrix)
        {
            return new Matrix(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41 * worldToRender.M11, matrix.M42 * worldToRender.M22, matrix.M43 * worldToRender.M33, matrix.M44 * worldToRender.M44);
        }

        internal Matrix RenderToWorld(Matrix matrix)
        {
            return new Matrix(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41 * renderToWorld.M11, matrix.M42 * renderToWorld.M22, matrix.M43 * renderToWorld.M33, matrix.M44 * renderToWorld.M44);
        }

        internal PhysicsEngine2D(Microsoft.Xna.Framework.Game game) : base(game)
        {

        }

        #region Registries
        // Registries //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This region contains all the registry add and remove methods as well as the registry variables

        // Stores all collision callbacks in PhysicsBody key, list of callbacks form
        private Dictionary<PhysicsBody2D, List<Collision2D.OnCollision>> registery_CollisionCallbacks;
        // Stores all the active collisions in PhysicsBody key, list of collisions form
        private Dictionary<PhysicsBody2D, List<Collision2D>> registery_ActiveCollisions;

        /// <summary>
        /// Adds a collision delegate to the physics bodies list of callbacks
        /// This enables something using the delegate to be alerted when the body in question collides with something else
        /// </summary>
        /// <param name="callback">The delegate to be added</param>
        /// <param name="body">The body to add the delegate to</param>
        internal void RegisterCollisionCallback(Collision2D.OnCollision callback, PhysicsBody2D body)
        {
            // First check to see if the registry contains this key
            if (!registery_CollisionCallbacks.ContainsKey(body))
            {
                // If the registry does not contain the body, add the body, and its list of callbacks
                registery_CollisionCallbacks.Add(body, body.collisionCallbacks);
            }
            // Add the new callback to the list
            body.collisionCallbacks.Add(callback);
        }

        /// <summary>
        /// Removes a collision delegate from the physics body
        /// </summary>
        /// <param name="callback">The delegate to be removed</param>
        /// <param name="body">The body to remove the delegate from</param>
        internal void UnregisterCollisionCallback(Collision2D.OnCollision callback, PhysicsBody2D body)
        {
            // First check to see if the registry contains this key
            if (registery_CollisionCallbacks.ContainsKey(body))
            {
                body.collisionCallbacks.Remove(callback);
            }
        }

        /// <summary>
        /// Given a body get all its collision callbacks
        /// </summary>
        /// <param name="body">The body to get callbacks from</param>
        /// <returns></returns>
        internal List<Collision2D.OnCollision> GetCollisionCallbacks(PhysicsBody2D body)
        {
            // First check to see if the registry contains this key
            if (registery_CollisionCallbacks.ContainsKey(body))
            {
                // If it has this body, get its list
                return registery_CollisionCallbacks[body];
            }
            else
                return null;
        }
        // End of Registries ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Bodies
        // Bodies ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This region contains all the body methods as well as the body variables

        private List<PhysicsBody2D> bodies_All;
        private List<PhysicsBody2D> bodies_Active;
        private List<PhysicsBody2D> bodies_Dead;

        internal void RemoveBody(PhysicsBody2D body)
        {
            bodies_Dead.Add(body);
        }
        // End of Bodies ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Broad Phase
        // Broad Phase //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This region contains all the broad phase members and functions

        // Use a list of BoundingChunks, for on the fly behaviour, but enable them to bind themselves to a dictionary of int -> boundingchunks
        // Store the first bounds position, assume it is index 0, then make every other bound calculate its index based around that position, and force all bodies that want to quickly check the bounds around themselves to do the same
        //PhysicsBoundingChunk[] bounds;
        List<PhysicsBoundingChunk2D> bounds;
        Dictionary<int, PhysicsBoundingChunk2D> bounds_hashtable;
        Transform bounds_transform;

        /// <summary>
        /// Adds a PhysicsBody to the Physics engine, bodies add themselves when made, this rarely needs to be called outside of the PhysicsBody constructor
        /// </summary>
        /// <param name="body">The body to be added</param>
        internal void AddBody(PhysicsBody2D body)
        {
            bodies_All.Add(body);

            if (body.flagBodyType.HasFlag(BodyType.STATIC))
            {
                if (bounds == null)
                {
                    bounds = new List<PhysicsBoundingChunk2D>();
                    bounds_hashtable = new Dictionary<int, PhysicsBoundingChunk2D>();

                    float x = (int)Math.Floor(body.transform.Position.X / PhysicsSettings.BOUNDINGBOX_LARGEST) * PhysicsSettings.BOUNDINGBOX_LARGEST;
                    float z = (int)Math.Floor(body.transform.Position.Z / PhysicsSettings.BOUNDINGBOX_LARGEST) * PhysicsSettings.BOUNDINGBOX_LARGEST;

                    bounds_transform = new Transform();
                    bounds_transform.Position = new Vector3(x, 0, z);

                    bounds.Add(new PhysicsBoundingChunk2D(bounds_transform));
                    bounds_hashtable.Add(CalculateBoundsIndex(bounds_transform), bounds[0]);
                    bounds[bounds.Count - 1].AddBody(body);

                    // If the body is so large that it takes up more than 1 chunk I need to know that, and build all the chunks it is in around the one at its center
                    int dimension = 3;
                    int count = 0;
                    bool tobig = false;
                    Transform newTransform = new Transform();
                    while (!tobig)
                    {
                        for (int i_x = 0; i_x < dimension; ++i_x)
                        {
                            for (int i_z = 0; i_z < dimension; ++i_z)
                            {
                                if (i_x == 0 || i_x == dimension - 1 || i_z == 0 || i_z == dimension - 1)
                                {
                                    newTransform.Transformation = Matrix.Identity;
                                    newTransform.Position = new Vector3((i_x - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST, 0, (i_z - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST) - bounds_transform.Position;

                                    PhysicsBoundingChunk2D newChunk = new PhysicsBoundingChunk2D(newTransform);
                                    if (newChunk.BoundsTest(body))
                                    {
                                        bounds.Add(new PhysicsBoundingChunk2D(bounds_transform));
                                        bounds_hashtable.Add(CalculateBoundsIndex(newTransform), newChunk);
                                        newChunk.AddBody(body);
                                        count++;
                                    }
                                }
                            }
                        }

                        tobig = (count == 0) ? true : false;

                        count = 0;
                        dimension += 2;
                    }
                }
                else
                {
                    int index = CalculateBoundsIndex(body.transform);

                    float x = (int)Math.Floor(body.transform.Position.X / PhysicsSettings.BOUNDINGBOX_LARGEST) * PhysicsSettings.BOUNDINGBOX_LARGEST;
                    float z = (int)Math.Floor(body.transform.Position.Z / PhysicsSettings.BOUNDINGBOX_LARGEST) * PhysicsSettings.BOUNDINGBOX_LARGEST;

                    bounds_transform = new Transform();
                    bounds_transform.Position = new Vector3(x, 0, z);

                    if (bounds_hashtable.ContainsKey(index))
                    {
                        bounds_hashtable[index].AddBody(body);
                    }
                    else
                    {
                        bounds.Add(new PhysicsBoundingChunk2D(bounds_transform));
                        bounds_hashtable.Add(CalculateBoundsIndex(bounds_transform), bounds[0]);
                        bounds[bounds.Count - 1].AddBody(body);
                    }

                    // If the body is so large that it takes up more than 1 chunk I need to know that, and build all the chunks it is in around the one at its center
                    int dimension = 3;
                    int count = 0;
                    bool tobig = false;
                    Transform newTransform = new Transform();
                    while (!tobig)
                    {
                        for (int i_x = 0; i_x < dimension; ++i_x)
                        {
                            for (int i_z = 0; i_z < dimension; ++i_z)
                            {
                                if (i_x == 0 || i_x == dimension - 1 || i_z == 0 || i_z == dimension - 1)
                                {
                                    newTransform.Transformation = Matrix.Identity;
                                    newTransform.Position = new Vector3((i_x - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST + x * PhysicsSettings.BOUNDINGBOX_LARGEST, 0, (i_z - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST + z * PhysicsSettings.BOUNDINGBOX_LARGEST) - bounds_transform.Position;
                                    int newIndex = CalculateBoundsIndex(newTransform);

                                    if (bounds_hashtable.ContainsKey(newIndex))
                                    {
                                        if (bounds_hashtable[newIndex].BoundsTest(body))
                                        {
                                            bounds_hashtable[newIndex].AddBody(body);
                                            count++;
                                        }
                                    }
                                    else
                                    {
                                        PhysicsBoundingChunk2D newChunk = new PhysicsBoundingChunk2D(newTransform);
                                        if (newChunk.BoundsTest(body))
                                        {
                                            bounds.Add(new PhysicsBoundingChunk2D(bounds_transform));
                                            bounds_hashtable.Add(newIndex, newChunk);
                                            newChunk.AddBody(body);
                                            count++;
                                        }
                                    }
                                }
                            }
                        }

                        tobig = (count == 0) ? true : false;

                        count = 0;
                        dimension += 2;
                    }
                }
            }
            else
            {
                bodies_Active.Add(body);
            }
        }

        private int CalculateBoundsIndex(Transform transform)
        {
            int x = (int)Math.Floor(transform.Position.X / PhysicsSettings.BOUNDINGBOX_LARGEST);
            int z = (int)Math.Floor(transform.Position.Z / PhysicsSettings.BOUNDINGBOX_LARGEST);

            return (x - (int)bounds_transform.Position.X) + (z - (int)bounds_transform.Position.Z) * (int)Math.Sqrt(int.MaxValue / 2);
        }

        internal List<int> CalculateBoundsIndices(PhysicsBody2D body)
        {
            List<int> indices = new List<int>();
            AABB boundingBox = body.shape.GetBoundingBox();
            int dimension = (int)Math.Max(boundingBox.Dimensions().X, boundingBox.Dimensions().Z) / 4;

            int x = (int)Math.Floor(body.transform.Position.X / PhysicsSettings.BOUNDINGBOX_LARGEST);
            int z = (int)Math.Floor(body.transform.Position.Z / PhysicsSettings.BOUNDINGBOX_LARGEST);

            indices.Add((x - (int)bounds_transform.Position.X) + (z - (int)bounds_transform.Position.Z) * (int)Math.Sqrt(int.MaxValue / 2));

            for (int i_x = 0; i_x < dimension; ++i_x)
            {
                for (int i_z = 0; i_z < dimension; ++i_z)
                {
                    if (i_x == 0 || i_x == dimension - 1 || i_z == 0 || i_z == dimension - 1)
                    {
                        Transform newTransform = new Transform();
                        newTransform.Transformation = Matrix.Identity;
                        newTransform.Position = new Vector3((i_x - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST + x * PhysicsSettings.BOUNDINGBOX_LARGEST, 0, (i_z - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST + z * PhysicsSettings.BOUNDINGBOX_LARGEST) - bounds_transform.Position;
                        int newIndex = CalculateBoundsIndex(newTransform);
                        if (!indices.Contains(newIndex))
                            indices.Add(newIndex);
                    }
                }
            }

            return indices;
        }

        // End of Broad Phase ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region GameComponent
        // GameComponent ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This region contains all the GameComponent overrides

        private Microsoft.Xna.Framework.Game game;

        /// <summary>
        /// Initialises the Physics class, this is what enables this class to be used statically
        /// </summary>
        public override void Initialize()
        {
            registery_CollisionCallbacks = new Dictionary<PhysicsBody2D, List<Collision2D.OnCollision>>();
            registery_ActiveCollisions = new Dictionary<PhysicsBody2D, List<Collision2D>>();

            bodies_All = new List<PhysicsBody2D>();
            bodies_Active = new List<PhysicsBody2D>();
            bodies_Dead = new List<PhysicsBody2D>();

            // Jenky as magic numbers, sorry. Works out this way for the scale we decided on in 3DSMax.
            worldToRender = Matrix.CreateScale(PhysicsSettings.MODEL_SCALE);
            renderToWorld = Matrix.CreateScale(1.0f / PhysicsSettings.MODEL_SCALE);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach(PhysicsBody2D body in bodies_Active)
            {
                body.Update();
            }

            // Remove all dead bodies
            foreach (PhysicsBody2D body in bodies_Dead)
            {
                bodies_All.Remove(body);
                bodies_Active.Remove(body);

                // If they have registered callbacks remove them
                if (registery_CollisionCallbacks.ContainsKey(body))
                {
                    registery_CollisionCallbacks.Remove(body);
                }

                if (body.flagBodyType.HasFlag(BodyType.STATIC))
                {
                    foreach (PhysicsBoundingChunk2D chunk in body.chunks)
                    {
                        chunk.RemoveBody(body);
                    }

                    body.chunks = null;
                }
            }
        }
        // End of GameComponent /////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
