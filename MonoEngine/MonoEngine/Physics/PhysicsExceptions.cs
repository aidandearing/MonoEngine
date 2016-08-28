using System;
using System.Runtime.Serialization;

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

        public class NotImplementedYet : Exception
        {
            public NotImplementedYet()
            {
            }

            public NotImplementedYet(string message) : base(message)
            {
            }

            public NotImplementedYet(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected NotImplementedYet(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        public class UnsupportedEngine : Exception
        {
            public UnsupportedEngine()
            {
            }

            public UnsupportedEngine(string message) : base(message)
            {
            }

            public UnsupportedEngine(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected UnsupportedEngine(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
