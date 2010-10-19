namespace OpenRasta.DI
{
    public enum DependencyLifetime
    {
        /// <summary>
        /// One object will be created every time it is requested in a constructor
        /// </summary>
        Transient,

        /// <summary>
        /// Only one of this type of object will ever be created and will last as long as the AppDomain.
        /// </summary>
        Singleton,

        /// <summary>
        /// One object will be created per request made to OpenRasta. It will be released at the end of the current request.
        /// </summary>
        PerRequest
    }
}