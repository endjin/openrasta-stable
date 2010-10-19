namespace OpenRasta.DI.Windsor
{
    #region Using Directives

    using System;

    using Castle.MicroKernel.Context;
    using Castle.MicroKernel.Lifestyle;

    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.DI.Internal;
    using OpenRasta.Exceptions;

    #endregion

    public class ContextStoreLifetime : AbstractLifestyleManager, IContextStoreDependencyCleaner
    {
        private bool registeredForCleanup;

        public void Destruct(string key, object instance)
        {
            base.Release(instance);
            this.GetStore()[key] = null;
        }

        public override object Resolve(CreationContext context)
        {
            var store = this.GetStore();

            var instance = base.Resolve(context);

            if (instance == null)
            {
                if (context.Handler.ComponentModel.ExtendedProperties[Constants.RegIsInstanceKey] != null)
                {
                    throw new DependencyResolutionException("Cannot find the instance in the context store.");
                }
            }
            else if (store[Model.Name] == null)
            {
                store[Model.Name] = instance;
                store.GetContextInstances().Add(new ContextStoreDependency(Model.Name, instance, this));
                this.registeredForCleanup = true;
            }

            if (!this.registeredForCleanup)
            {
                store.GetContextInstances().Add(new ContextStoreDependency(Model.Name, instance, this));
                this.registeredForCleanup = true;
            }

            return store[Model.Name];
        }

        public override bool Release(object instance)
        {
            return false;
        }

        public override void Dispose()
        {
        }

        private IContextStore GetStore()
        {
            if (!Kernel.HasComponent(typeof(IContextStore)))
            {
                throw new InvalidOperationException(
                    "There is no IContextStore implementation registered in the container.");
            }

            return Kernel.Resolve<IContextStore>();
        }
    }
}