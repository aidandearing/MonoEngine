﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoEngine.Shapes;

namespace MonoEngine.Physics.Physics2D
{
    /// <summary>
    /// Contains an array of Bounding Boxes that partition the worldspace in order to narrow the total number of collision checks any one non-static body must check against
    /// </summary>
    public class PhysicsBoundingChunk2D
    {
        private AABB[] bounds;
        private Transform transform;
        private Dictionary<AABB, List<PhysicsBody2D>> statics;
        private Dictionary<AABB, List<PhysicsBody2D>> dynamics;
        private Dictionary<int, int> indexToOrder;
        private Dictionary<int, List<AABB>> orderToIndex;
        private Dictionary<AABB, int> boundToOrder;
        private static int sum;
        private static int[] bound_dim;

        public PhysicsBoundingChunk2D(Transform transform)
        {
            // Place the bounding chunk centered at the passed transform (which is placed on a grid of points rounded to the nearest point at the dimensions of the chunk (so no chunks overlap))
            this.transform = transform;

            // Instantiate all the dictionaries
            statics = new Dictionary<AABB, List<PhysicsBody2D>>();
            dynamics = new Dictionary<AABB, List<PhysicsBody2D>>();
            indexToOrder = new Dictionary<int, int>();
            orderToIndex = new Dictionary<int, List<AABB>>();
            boundToOrder = new Dictionary<AABB, int>();

            // Calculate the sum of powers for the bounding box array

            // This is the total number of bounding boxes that will be present
            if (sum == 0)
            {
                sum = ((int)Math.Pow(4, PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS) - 1) / (4 - 1);
            }

            // Instantiate the bounds array
            bounds = new AABB[sum];

            // Instantiate the bounds dimensions array
            if (bound_dim == null)
            {
                bound_dim = new int[PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS];
            }

            for (int i = PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS; i > 0; i--)
            {
                // Assign the dimension values for each order of bounding boxes to the array of dimensions
                bound_dim[PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS - i] = (int)Math.Pow(PhysicsEngine.PhysicsSettings.BOUNDINGBOX_SMALLEST, i);
            }

            // Store the current index in the array of bounds
            int index = 0;
            // Iterate the depth of the bounds
            for (int i = 0; i < PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS; ++i)
            {
                // Prepare a list of all bounds at this depth
                List<AABB> orderList = new List<AABB>();
                // Add the list and the depth to the order dictionary
                orderToIndex.Add(i, orderList);

                // Calculate the total number of bounds to be made at this depth
                int numberAtOrder = (int)Math.Pow(4, i);
                // Calculate the width of bounding boxes at this depth (not their dimensional width, but the actual number of bounds across at this depth)
                int width = (int)Math.Pow(2, i);

                // Iterate for the total number of bounds to be made at this depth
                for (int j = 0; j < numberAtOrder; ++j)
                {
                    // Add the current index and the current order to the index dictionary
                    indexToOrder.Add(index, i);

                    // Calculate the position of this bounding box
                    Vector3 pos = (new Vector3(j % width, 0, (int)(j / width)) - new Vector3((numberAtOrder - 1) / width, 0, (numberAtOrder - 1) / width) / 2) * bound_dim[i];

                    // Create the transform for this bounding box
                    Transform trans = new Transform(transform.Transformation.Translation + pos, transform.Transformation.Scale, transform.Transformation.Rotation);
                    // Instantiate the bounding box with the transform, and the dimensions necessary
                    bounds[index] = new AABB(trans, bound_dim[i], bound_dim[i]);
                    // Add the current bounds and order to the bounds to order dictionary
                    boundToOrder.Add(bounds[index], i);
                    // Add the bounding box to the dictionary of bounding box | list of bodies
                    statics.Add(bounds[index], new List<PhysicsBody2D>());
                    dynamics.Add(bounds[index], new List<PhysicsBody2D>());

                    // Add the bounding box to the list of boxes at this depth
                    orderList.Add(bounds[index]);
                    // Increment the index so no other bounding box overwrites any other bounding box in the array
                    index++;
                }
            }
        }

        public bool BoundsTest(PhysicsBody2D body)
        {
            return bounds[0].OverlapTest(body.shape, bounds[0].transform, body.transform);
        }

        /// <summary>
        /// Attempts to add a body to the bounding chunk, the criteria for failure are:
        /// The body is not static
        /// The body is not overlapping the bounds
        /// </summary>
        /// <param name="body">The body to add</param>
        /// <returns>True on success, False on failure</returns>
        public bool AddBody(PhysicsBody2D body)
        {
            bool added = false;

            //for (int i = 0; i < PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS; ++i)
            //{
            //    foreach (AABB bounds in orderToIndex[i])
            //    {
            //        if (bounds.OverlapTest(body.shape))
            //        {
            //            if (body.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC))
            //            {
            //                statics[bounds].Add(body);
            //            }
            //            else
            //            {
            //                dynamics[bounds].Add(body);
            //            }
            //            body.chunks.Add(this);
            //            added = true;
            //        }
            //    }
            //}

            for (int i = ShapeToOrder(body.shape); i < PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS; ++i)
            {
                foreach (AABB bounds in orderToIndex[i])
                {
                    if (bounds.OverlapTest(body.shape, bounds.transform, body.transform))
                    {
                        if (body.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC))
                        {
                            if (!statics[bounds].Contains(body))
                                statics[bounds].Add(body);
                        }
                        else
                        {
                            if (!dynamics[bounds].Contains(body))
                                dynamics[bounds].Add(body);
                        }
                        body.chunks.Add(this);
                        added = true;
                    }
                }
            }

            return added;
        }

        // TODO RemoveBody from PhysicsBoundingChunk2D can probably be way better
        public void RemoveBody(PhysicsBody2D body)
        {
            foreach (AABB bounds in bounds)
            {
                if (body.flagBodyType.HasFlag(PhysicsEngine.BodyType.STATIC))
                {
                    if (statics[bounds].Contains(body))
                        statics[bounds].Remove(body);
                }
                else
                {
                    if (dynamics[bounds].Contains(body))
                        dynamics[bounds].Remove(body);
                }
            }
        }

        /// <summary>
        /// Given a body this method returns all bodies around that body in this bounding chunk
        /// </summary>
        /// <param name="body">The body to get neighbours of</param>
        /// <returns>Either a list of bodies, if the body is within this bounding chunk, or an empty list, if it is not</returns>
        public List<PhysicsBody2D> GetNearbyBodies(PhysicsBody2D body)
        {
            List<PhysicsBody2D> bodies = new List<PhysicsBody2D>();

            // Since every AABB in this chunk has a list that contains all the objects in that AABB and all the children AABB I only need to check against the AABBs at the same 'order' as the bodies shape
            for (int i = ShapeToOrder(body.shape); i < PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS; ++i)
            {
                foreach (AABB bounds in orderToIndex[i])
                {
                    if (bounds.OverlapTest(body.shape, bounds.transform, body.transform))
                    {
                        bodies.AddRange(statics[bounds]);
                        bodies.AddRange(dynamics[bounds]);
                    }
                }
            }

            return bodies;
        }

        public static int ShapeToOrder(Shape shape)
        {
            float maxDim = MathHelper.Max(shape.GetBoundingBox().Dimensions.X, shape.GetBoundingBox().Dimensions.Z);

            // Iterate from largest bounds to smallest, until one is smaller than the maxDim
            int depth = 0;
            while (depth < PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS)
            {
                if (bound_dim[depth] < maxDim)
                    return MathHelper.Max(depth - 1, 0);
                else
                    depth++;
            }

            return PhysicsEngine.PhysicsSettings.BOUNDINGBOX_ORDERS - 1;
        }
    }
}
