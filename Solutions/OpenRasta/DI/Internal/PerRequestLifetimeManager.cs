namespace OpenRasta.DI.Internal
{
    #region Using Directives

    using System.Linq;

    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Exceptions;

    #endregion

    public class PerRequestLifetimeManager : DependencyLifetimeManager
    {
        public PerRequestLifetimeManager(InternalDependencyResolver resolver) : base(resolver)
        {
        }

        public override bool IsRegistrationAvailable(DependencyRegistration registration)
        {
            if (!Resolver.HasDependency(typeof(IContextStore)))
            {
                return false;
            }

            if (!registration.IsInstanceRegistration)
            {
                return true;
            }

            var store = Resolver.Resolve<IContextStore>();

            bool storeHasRegistration = store.GetContextInstances().Any(x => x.Key == registration.Key);
            
            return storeHasRegistration;
        }

        public override object Resolve(ResolveContext context, DependencyRegistration registration)
        {
            this.CheckContextStoreAvailable();

            object instance;
            var contextStore = Resolver.Resolve<IContextStore>();

            if ((instance = contextStore[registration.Key]) == null)
            {
                if (registration.IsInstanceRegistration)
                {
                    throw new DependencyResolutionException(
                        "A dependency registered as an instance wasn't found. The registration was removed.");
                }

                instance = base.Resolve(context, registration);

                this.StoreInstanceInContext(contextStore, registration.Key, instance);
            }

            return instance;
        }

        public override void VerifyRegistration(DependencyRegistration registration)
        {
            if (registration.IsInstanceRegistration)
            {
                this.CheckContextStoreAvailable();
                var instance = registration.Instance;
                registration.Instance = null;
                var store = Resolver.Resolve<IContextStore>();
                
                if (store[registration.Key] != null)
                {
                    throw new DependencyResolutionException(
                        "An instance is being registered for an existing registration.");
                }

                this.StoreInstanceInContext(store, registration.Key, instance);
            }
        }

        private void CheckContextStoreAvailable()
        {
            if (!Resolver.HasDependency(typeof(IContextStore)))
            {
                throw new DependencyResolutionException(
                    "Could not resolve the context store. Make sure you have registered one.");
            }
        }

        private void StoreInstanceInContext(IContextStore contextStore, string key, object instance)
        {
            contextStore[key] = instance;
            contextStore.GetContextInstances().Add(new ContextStoreDependency(key, instance, Resolver.Registrations));
        }
    }
}