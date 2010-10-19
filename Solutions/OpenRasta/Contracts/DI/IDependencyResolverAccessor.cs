namespace OpenRasta.Contracts.DI
{
    /// <summary>
    /// Provides an instance of the dependency resolver to be used with OpenRasta.
    /// </summary>
    public interface IDependencyResolverAccessor
    {
        IDependencyResolver Resolver { get; }
    }
}