﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Physics
{
    public class PhysicsExceptions
    {
        public class InvalidPhysicsBody : Exception
        {
            public InvalidPhysicsBody()
            {
            }

            public InvalidPhysicsBody(string message) : base(message)
            {
            }

            public InvalidPhysicsBody(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected InvalidPhysicsBody(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        public class InvalidCollisionCallback : Exception
        {
            public InvalidCollisionCallback()
            {
            }

            public InvalidCollisionCallback(string message) : base(message)
            {
            }

            public InvalidCollisionCallback(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected InvalidCollisionCallback(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}