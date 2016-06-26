using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace Capstone
{
    class Physics : GameComponent
    {
        private Matrix worldToRender;
        private Matrix renderToWorld;

        public static Matrix WorldToRender(Matrix matrix)
        {
            return new Matrix(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41 * instance.worldToRender.M11, matrix.M42 * instance.worldToRender.M22, matrix.M43 * instance.worldToRender.M33, matrix.M44 * instance.worldToRender.M44);
        }

        public static Matrix RenderToWorld(Matrix matrix)
        {
            return new Matrix(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41 * instance.renderToWorld.M11, matrix.M42 * instance.renderToWorld.M22, matrix.M43 * instance.renderToWorld.M33, matrix.M44 * instance.renderToWorld.M44);
        }

        #region Singleton
        // Singleton ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This region contains all the singleton methods and variables

        private static Physics instance;
        /// <summary>
        /// DO NOT USE THIS
        /// Unless you are stupid, or doing something sweet
        /// </summary>
        /// <param name="game">The Game instance running</param>
        /// <returns>The Physics instance running</returns>
        public static Physics Instance(Game game)
        {
            instance = (instance == null) ? new Physics(game) : instance;
            return instance;
        }
        private Physics(Game game) : base(game)
        {
            
        }
        // End of Singleton ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Registries
        // Registries //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This region contains all the registry add and remove methods as well as the registry variables

        // Stores all collision callbacks in PhysicsBody key, list of callbacks form
        private Dictionary<PhysicsBody, List<Collision.OnCollision>> registery_CollisionCallbacks;
        // Stores all the active collisions in PhysicsBody key, list of collisions form
        private Dictionary<PhysicsBody, List<Collision>> registery_ActiveCollisions;

        /// <summary>
        /// Adds a collision delegate to the physics bodies list of callbacks
        /// This enables something using the delegate to be alerted when the body in question collides with something else
        /// </summary>
        /// <param name="callback">The delegate to be added</param>
        /// <param name="body">The body to add the delegate to</param>
        public static void RegisterCollisionCallback(Collision.OnCollision callback, PhysicsBody body)
        {
            // First check to see if the registry contains this key
            if (!instance.registery_CollisionCallbacks.ContainsKey(body))
            {
                // If the registry does not contain the body, add the body, and its list of callbacks
                instance.registery_CollisionCallbacks.Add(body, body.collisionCallbacks);
            }
            // Add the new callback to the list
            body.collisionCallbacks.Add(callback);
        }

        /// <summary>
        /// Removes a collision delegate from the physics body
        /// </summary>
        /// <param name="callback">The delegate to be removed</param>
        /// <param name="body">The body to remove the delegate from</param>
        public static void UnregisterCollisionCallback(Collision.OnCollision callback, PhysicsBody body)
        {
            // First check to see if the registry contains this key
            if (instance.registery_CollisionCallbacks.ContainsKey(body))
            {
                body.collisionCallbacks.Remove(callback);
            }
        }

        /// <summary>
        /// Given a body get all its collision callbacks
        /// </summary>
        /// <param name="body">The body to get callbacks from</param>
        /// <returns></returns>
        public static List<Collision.OnCollision> GetCollisionCallbacks(PhysicsBody body)
        {
            // First check to see if the registry contains this key
            if (instance.registery_CollisionCallbacks.ContainsKey(body))
            {
                // If it has this body, get its list
                return instance.registery_CollisionCallbacks[body];
            }
            else
                return null;
        }
        // End of Registries ////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Bodies
        // Bodies ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This region contains all the body methods as well as the body variables

        private List<PhysicsBody> bodies_All;
        private List<PhysicsBody> bodies_Active;
        private List<PhysicsBody> bodies_Dead;

        public static void RemovePhysicsBody(PhysicsBody body)
        {
            instance.bodies_Dead.Add(body);
        }
        // End of Bodies ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Broad Phase
        // Broad Phase //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // This region contains all the broad phase members and functions

        // TODO decide on how to build these, post scene load, but pre simulation, on the fly, or explicitly?
        // Post scene load:
        // Compute the bounds of all static bodies and build the array of BoundingChunks to match this, using a method call
        // Much like Explicity this has the added bonus of allowing us to quickly compute the general area of boundingchunks to check based on a non static bodies transform
        // On the fly:
        // As static bodies are added find out if they are within any preexisting bounding chunks, if they aren't, build a new one that fits on a grid with the others, and put the body in it
        // This has very few optimisations based on transform, but can allow the world to easily take on any shape and still function without wasting time with bounding chunks that are empty, due to being a rigid rectangle of chunks
        // Explicity:
        // Force the coder to call a method that requires the exact dimensions of the world and builds the array of bounding chunks appropriately
        // Much like Post scene load the benefit comes in knowing where to look in the array given the transform of a body

        // Another option!
        // Use a list of BoundingChunks, for on the fly behaviour, but enable them to bind themselves to a dictionary of int -> boundingchunks
        // Store the first bounds position, assume it is index 0, then make every other bound calculate its index based around that position, and force all bodies that want to quickly check the bounds around themselves to do the same
        //PhysicsBoundingChunk[] bounds;
        List<PhysicsBoundingChunk> bounds;
        Dictionary<int, PhysicsBoundingChunk> bounds_hashtable;
        Transform bounds_transform;

        /// <summary>
        /// Adds a PhysicsBody to the Physics engine, bodies add themselves when made, this rarely needs to be called outside of the PhysicsBody constructor
        /// </summary>
        /// <param name="body">The body to be added</param>
        public static void AddPhysicsBody(PhysicsBody body)
        {
            instance.bodies_All.Add(body);

            // TODO decide on the best way of handling how to add bodies to the PhysicsBoundingChunks
            // Static optimisations (BoundingChunks broadphase system)
            //if (body.flagBodyType.HasFlag(PhysicsBody.BodyType.physics_static))
            //{
            //    for (int i = 0; i < instance.bounds.Length; ++i)
            //    {
            //        instance.bounds[i].AddBody(body);
            //    }
            //}

            if (body.flagBodyType.HasFlag(PhysicsBody.BodyType.physics_static))
            {
                if (instance.bounds == null)
                {
                    instance.bounds = new List<PhysicsBoundingChunk>();
                    instance.bounds_hashtable = new Dictionary<int, PhysicsBoundingChunk>();

                    // TODO allow PhysicsBoundingChunks to be more adaptable, not so hard coded for our specific games case (maybe?)
                    float x = (int)Math.Floor(body.parent.transform.Position.X / 8) * 8;
                    float z = (int)Math.Floor(body.parent.transform.Position.Z / 8) * 8;

                    instance.bounds_transform = new Transform();
                    instance.bounds_transform.Position = new Vector3(x, 0, z);

                    instance.bounds.Add(new PhysicsBoundingChunk(instance.bounds_transform));
                    instance.bounds_hashtable.Add(CalculateBoundsIndex(instance.bounds_transform), instance.bounds[0]);
                    instance.bounds[instance.bounds.Count - 1].AddBody(body);

                    // If the body is so large that it takes up more than 1 chunk I need to know that, and build all the chunks it is in around the one at its center
                    // TODO Build functionality that enables large objects to still generate the appropriate amount of chunks to encase them entirely

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
                                    newTransform.Position = new Vector3((i_x - (int)Math.Floor(dimension / 2.0f)) * 8, 0, (i_z - (int)Math.Floor(dimension / 2.0f)) * 8) - instance.bounds_transform.Position;

                                    PhysicsBoundingChunk newChunk = new PhysicsBoundingChunk(newTransform);
                                    if (newChunk.BoundsTest(body))
                                    {
                                        instance.bounds.Add(new PhysicsBoundingChunk(instance.bounds_transform));
                                        instance.bounds_hashtable.Add(CalculateBoundsIndex(newTransform), newChunk);
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
                    int index = CalculateBoundsIndex(body.parent.transform);

                    float x = (int)Math.Floor(body.parent.transform.Position.X / 8) * 8;
                    float z = (int)Math.Floor(body.parent.transform.Position.Z / 8) * 8;

                    instance.bounds_transform = new Transform();
                    instance.bounds_transform.Position = new Vector3(x, 0, z);

                    if (instance.bounds_hashtable.ContainsKey(index))
                    {
                        instance.bounds_hashtable[index].AddBody(body);
                    }
                    else
                    {
                        instance.bounds.Add(new PhysicsBoundingChunk(instance.bounds_transform));
                        instance.bounds_hashtable.Add(CalculateBoundsIndex(instance.bounds_transform), instance.bounds[0]);
                        instance.bounds[instance.bounds.Count - 1].AddBody(body);
                    }

                    // If the body is so large that it takes up more than 1 chunk I need to know that, and build all the chunks it is in around the one at its center
                    // TODO Build functionality that enables large objects to still generate the appropriate amount of chunks to encase them entirely

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
                                    newTransform.Position = new Vector3((i_x - (int)Math.Floor(dimension / 2.0f)) * 8 + x * 8, 0, (i_z - (int)Math.Floor(dimension / 2.0f)) * 8 + z * 8) - instance.bounds_transform.Position;
                                    int newIndex = CalculateBoundsIndex(newTransform);

                                    if (instance.bounds_hashtable.ContainsKey(newIndex))
                                    {
                                        if (instance.bounds_hashtable[newIndex].BoundsTest(body))
                                        {
                                            instance.bounds_hashtable[newIndex].AddBody(body);
                                            count++;
                                        }
                                    }
                                    else
                                    {
                                        PhysicsBoundingChunk newChunk = new PhysicsBoundingChunk(newTransform);
                                        if (newChunk.BoundsTest(body))
                                        {
                                            instance.bounds.Add(new PhysicsBoundingChunk(instance.bounds_transform));
                                            instance.bounds_hashtable.Add(newIndex, newChunk);
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
        }

        private static int CalculateBoundsIndex(Transform transform)
        {
            int x = (int)Math.Floor(transform.Position.X / 8);
            int z = (int)Math.Floor(transform.Position.Z / 8);

            return (x - (int)instance.bounds_transform.Position.X) + (z - (int)instance.bounds_transform.Position.Z) * (int)Math.Sqrt(int.MaxValue / 2);
        }

        public static List<int> CalculateBoundsIndices(PhysicsBody body)
        {
            List<int> indices = new List<int>();
            AABB boundingBox = body.shape.GetBoundingBox();
            int dimension = (int)Math.Max(boundingBox.Dimensions().X, boundingBox.Dimensions().Z) / 4;

            int x = (int)Math.Floor(body.parent.transform.Position.X / 8);
            int z = (int)Math.Floor(body.parent.transform.Position.Z / 8);

            indices.Add((x - (int)instance.bounds_transform.Position.X) + (z - (int)instance.bounds_transform.Position.Z) * (int)Math.Sqrt(int.MaxValue / 2));

            for (int i_x = 0; i_x < dimension; ++i_x)
            {
                for (int i_z = 0; i_z < dimension; ++i_z)
                {
                    if (i_x == 0 || i_x == dimension - 1 || i_z == 0 || i_z == dimension - 1)
                    {
                        Transform newTransform = new Transform();
                        newTransform.Transformation = Matrix.Identity;
                        newTransform.Position = new Vector3((i_x - (int)Math.Floor(dimension / 2.0f)) * 8 + x * 8, 0, (i_z - (int)Math.Floor(dimension / 2.0f)) * 8 + z * 8) - instance.bounds_transform.Position;
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

        private Game game;

        /// <summary>
        /// Initialises the Physics class, this is what enables this class to be used statically
        /// </summary>
        public override void Initialize()
        {
            registery_CollisionCallbacks = new Dictionary<PhysicsBody, List<Collision.OnCollision>>();
            registery_ActiveCollisions = new Dictionary<PhysicsBody, List<Collision>>();

            bodies_All = new List<PhysicsBody>();
            bodies_Active = new List<PhysicsBody>();
            bodies_Dead = new List<PhysicsBody>();

            // Jenky as magic numbers, sorry. Works out this way for the scale we decided on in 3DSMax.
            worldToRender = Matrix.CreateScale(100f);
            renderToWorld = Matrix.CreateScale(1 / 100.0f);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Remove all dead bodies
            foreach (PhysicsBody body in bodies_Dead)
            {
                bodies_All.Remove(body);
                bodies_Active.Remove(body);

                // If they have registered callbacks remove them
                if (registery_CollisionCallbacks.ContainsKey(body))
                {
                    registery_CollisionCallbacks.Remove(body);
                }
            }
        }
        // End of GameComponent /////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
