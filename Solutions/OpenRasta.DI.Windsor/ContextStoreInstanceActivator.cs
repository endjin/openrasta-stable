namespace OpenRasta.DI.Windsor
{
    #region Using Directives

    using System.Diagnostics;

    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.ComponentActivator;
    using Castle.MicroKernel.Context;

    using OpenRasta.Contracts.Pipeline;

    #endregion

    public class ContextStoreInstanceActivator : AbstractComponentActivator
    {
        private string storeKey;

        public ContextStoreInstanceActivator(
            ComponentModel model, 
            IKernel kernel, 
            ComponentInstanceDelegate onCreation,
            ComponentInstanceDelegate onDestruction) : base(model, kernel, onCreation, onDestruction)
        {
            this.storeKey = model.Name;
        }

        protected override object InternalCreate(CreationContext context)
        {
            var store = (IContextStore)Kernel.Resolve(typeof(IContextStore));

            if (store[this.storeKey] == null)
            {
                Debug.WriteLine("The instance is not present in the context store");
                return null;
            }

            return store[this.storeKey];
        }

        protected override void InternalDestroy(object instance)
        {
            var store = (IContextStore)Kernel.Resolve(typeof(IContextStore));

            store[this.storeKey] = null;
        }
    }
}