﻿namespace OpenRasta.DI
{
    using OpenRasta.DI.Internal;

    public abstract class DependencyLifetimeManager
    {
        protected DependencyLifetimeManager(InternalDependencyResolver resolver)
        {
            this.Resolver = resolver;
        }

        protected InternalDependencyResolver Resolver { get; private set; }

        public virtual bool IsRegistrationAvailable(DependencyRegistration registration)
        {
            return true;
        }

        public virtual object Resolve(ResolveContext context, DependencyRegistration registration)
        {
            return context.Builder.CreateObject(registration);
        }

        public virtual void VerifyRegistration(DependencyRegistration registration)
        {
        }
    }
}