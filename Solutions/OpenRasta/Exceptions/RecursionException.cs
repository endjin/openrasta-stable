namespace OpenRasta.Exceptions
{
    using System;

    public class RecursionException : Exception
    {
        public RecursionException() : this("Recursion is not allowed.")
        {
        }

        public RecursionException(string message) : base(message)
        {
        }

        public RecursionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}