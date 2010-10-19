namespace OpenRasta.Web.UriTemplates
{
    using System;
    using System.Runtime.Serialization;

    public class UriTemplateMatchException : SystemException
    {
        public UriTemplateMatchException()
        {
        }

        public UriTemplateMatchException(string message) : base(message)
        {
        }

        public UriTemplateMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UriTemplateMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}