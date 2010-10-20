namespace OpenRasta.DI.Ninject
{
    using global::Ninject;

    using OpenRasta.Contracts.Pipeline;

    /// <summary>
    /// A class to clean items out of the <see cref="IContextStore"/>.
    /// </summary>
    public class ContextStoreDependencyCleaner : IContextStoreDependencyCleaner
    {
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextStoreDependencyCleaner"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public ContextStoreDependencyCleaner(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// Destructs the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="instance">The instance.</param>
        public void Destruct(string key, object instance)
        {
            var store = this.kernel.Get<IContextStore>();
            store[key] = null;
        }
    }
}