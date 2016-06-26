using Microsoft.Xna.Framework;
using System;

namespace MonoEngine
{
    namespace Physics3D
    {
        public class PhysicsEngine3D : GameComponent
        {
            public class PhysicsSettings
            {
                public static int BOUNDINGBOX_SMALLEST = 2;
                public static int BOUNDINGBOX_ORDERS = 4;
                public static int BOUNDINGBOX_LARGEST = (int)Math.Pow(BOUNDINGBOX_SMALLEST, BOUNDINGBOX_ORDERS);
            }

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

            private static PhysicsEngine3D instance;
            /// <summary>
            /// DO NOT USE THIS
            /// Unless you are stupid, or doing something sweet
            /// </summary>
            /// <param name="game">The Game instance running</param>
            /// <returns>The Physics instance running</returns>
            public static PhysicsEngine3D Instance(Microsoft.Xna.Framework.Game game)
            {
                instance = (instance == null) ? new PhysicsEngine3D(game) : instance;
                return instance;
            }
            private PhysicsEngine3D(Microsoft.Xna.Framework.Game game) : base(game)
            {

            }
            // End of Singleton ////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion
            
            /*
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
    */

            /*
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
    */

            /*
            #region Broad Phase
            // Broad Phase //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // This region contains all the broad phase members and functions

            // Use a list of BoundingChunks, for on the fly behaviour, but enable them to bind themselves to a dictionary of int -> boundingchunks
            // Store the first bounds position, assume it is index 0, then make every other bound calculate its index based around that position, and force all bodies that want to quickly check the bounds around themselves to do the same
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
                        float x = (int)Math.Floor(body.transform.Position.X / PhysicsSettings.BOUNDINGBOX_LARGEST) * PhysicsSettings.BOUNDINGBOX_LARGEST;
                        float z = (int)Math.Floor(body.transform.Position.Z / PhysicsSettings.BOUNDINGBOX_LARGEST) * PhysicsSettings.BOUNDINGBOX_LARGEST;

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
                                        newTransform.Position = new Vector3((i_x - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST, 0, (i_z - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST) - instance.bounds_transform.Position;

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
                        int index = CalculateBoundsIndex(body.transform);

                        float x = (int)Math.Floor(body.transform.Position.X / PhysicsSettings.BOUNDINGBOX_LARGEST) * PhysicsSettings.BOUNDINGBOX_LARGEST;
                        float z = (int)Math.Floor(body.transform.Position.Z / PhysicsSettings.BOUNDINGBOX_LARGEST) * PhysicsSettings.BOUNDINGBOX_LARGEST;

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
                                        newTransform.Position = new Vector3((i_x - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST + x * PhysicsSettings.BOUNDINGBOX_LARGEST, 0, (i_z - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST + z * PhysicsSettings.BOUNDINGBOX_LARGEST) - instance.bounds_transform.Position;
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
                int x = (int)Math.Floor(transform.Position.X / PhysicsSettings.BOUNDINGBOX_LARGEST);
                int z = (int)Math.Floor(transform.Position.Z / PhysicsSettings.BOUNDINGBOX_LARGEST);

                return (x - (int)instance.bounds_transform.Position.X) + (z - (int)instance.bounds_transform.Position.Z) * (int)Math.Sqrt(int.MaxValue / 2);
            }

            public static List<int> CalculateBoundsIndices(PhysicsBody body)
            {
                List<int> indices = new List<int>();
                AABB boundingBox = body.shape.GetBoundingBox();
                int dimension = (int)Math.Max(boundingBox.Dimensions().X, boundingBox.Dimensions().Z) / 4;

                int x = (int)Math.Floor(body.transform.Position.X / PhysicsSettings.BOUNDINGBOX_LARGEST);
                int z = (int)Math.Floor(body.transform.Position.Z / PhysicsSettings.BOUNDINGBOX_LARGEST);

                indices.Add((x - (int)instance.bounds_transform.Position.X) + (z - (int)instance.bounds_transform.Position.Z) * (int)Math.Sqrt(int.MaxValue / 2));

                for (int i_x = 0; i_x < dimension; ++i_x)
                {
                    for (int i_z = 0; i_z < dimension; ++i_z)
                    {
                        if (i_x == 0 || i_x == dimension - 1 || i_z == 0 || i_z == dimension - 1)
                        {
                            Transform newTransform = new Transform();
                            newTransform.Transformation = Matrix.Identity;
                            newTransform.Position = new Vector3((i_x - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST + x * PhysicsSettings.BOUNDINGBOX_LARGEST, 0, (i_z - (int)Math.Floor(dimension / 2.0f)) * PhysicsSettings.BOUNDINGBOX_LARGEST + z * PhysicsSettings.BOUNDINGBOX_LARGEST) - instance.bounds_transform.Position;
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
    */

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

                // Jenky as magic numbers, sorry. Works out this way for the scale we decided on in 3DSMax.
                worldToRender = Matrix.CreateScale(200f);
                renderToWorld = Matrix.CreateScale(1 / 200.0f);

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
}
