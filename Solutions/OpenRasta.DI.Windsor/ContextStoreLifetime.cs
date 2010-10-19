using System;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;
using OpenRasta.DI.Internal;
using OpenRasta.Pipeline;

namespace OpenRasta.DI.Windsor
{
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Exceptions;

    public class ContextStoreLifetime : AbstractLifestyleManager, IContextStoreDependencyCleaner
    {
        bool _isRegisteredForCleanup;

        public void Destruct(string key, object instance)
        {
            base.Release(instance);
            GetStore()[key] = null;
        }

        public override object Resolve(CreationContext context)
        {
            var store = GetStore();

            var instance = base.Resolve(context);

            if (instance == null)
            {
                if (context.Handler.ComponentModel.ExtendedProperties[Constants.REG_IS_INSTANCE_KEY] != null)
                {
                    throw new DependencyResolutionException("Cannot find the instance in the context store.");
                }
            }
            else if (store[Model.Name] == null)
            {
                store[Model.Name] = instance;
                store.GetContextInstances().Add(new ContextStoreDependency(Model.Name, instance, this));
                _isRegisteredForCleanup = true;
            }

            if (!_isRegisteredForCleanup)
            {
                store.GetContextInstances().Add(new ContextStoreDependency(Model.Name, instance, this));
                _isRegisteredForCleanup = true;
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

        IContextStore GetStore()
        {
            if (!Kernel.HasComponent(typeof (IContextStore)))
                throw new InvalidOperationException(
                    "There is no IContextStore implementation registered in the container.");
            return Kernel.Resolve<IContextStore>();
        }
    }
}