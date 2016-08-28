using System;
using System.Runtime.Serialization;

namespace MonoEngine.Render
{
    public class RenderExceptions
    {
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
    }
}
