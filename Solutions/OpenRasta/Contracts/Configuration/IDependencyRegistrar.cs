namespace OpenRasta.Contracts.Configuration
{
    using OpenRasta.Contracts.DI;

    /// <summary>
    /// Provides the initial set of dependencies required to initialize OpenRasta.
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// Registers the default dependencies.
        /// </summary>
        void Register(IDependencyResolver resolver);
    }
}