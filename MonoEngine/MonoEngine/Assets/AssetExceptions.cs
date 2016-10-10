using System;
using System.Runtime.Serialization;

namespace MonoEngine.Assets
{
    public class AssetExceptions
    {
        [Serializable]
        public class AssetAlreadyExists : Exception
        {
            public AssetAlreadyExists()
            {
            }

            public AssetAlreadyExists(string message) : base(message)
            {
            }

            public AssetAlreadyExists(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected AssetAlreadyExists(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        public class TransformFromXMLFormat : Exception
        {
            public TransformFromXMLFormat()
            {
            }

            public TransformFromXMLFormat(string message) : base(message)
            {
            }

            public TransformFromXMLFormat(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected TransformFromXMLFormat(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        public class ResourceManagerOfTypeAlreadyExists : Exception
        {
            public ResourceManagerOfTypeAlreadyExists()
            {
            }

            public ResourceManagerOfTypeAlreadyExists(string message) : base(message)
            {
            }

            public ResourceManagerOfTypeAlreadyExists(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ResourceManagerOfTypeAlreadyExists(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
