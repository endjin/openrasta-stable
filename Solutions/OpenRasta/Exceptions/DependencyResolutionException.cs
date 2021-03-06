﻿namespace OpenRasta.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DependencyResolutionException : Exception
    {
        public DependencyResolutionException()
        {
        }

        public DependencyResolutionException(string message)
            : base(message)
        {
        }

        public DependencyResolutionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DependencyResolutionException(
            SerializationInfo info, 
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}