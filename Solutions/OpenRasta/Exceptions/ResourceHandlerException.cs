namespace OpenRasta.Exceptions
{
    using System;

    [Serializable]
    public class ResourceHandlerException : Exception
    {
        public ResourceHandlerException()
        {
        }

        public ResourceHandlerException(string message) : base(message)
        {
        }

        public ResourceHandlerException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ResourceHandlerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}