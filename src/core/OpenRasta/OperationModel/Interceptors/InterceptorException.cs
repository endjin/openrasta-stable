namespace OpenRasta.OperationModel.Interceptors
{
    using System;

    public class InterceptorException : Exception
    {
        public InterceptorException(string message) : base(message)
        {
        }

        public InterceptorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}