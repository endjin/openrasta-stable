namespace OpenRasta.DI.Internal
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.Diagnostics;

    public class ResolveContext
    {
        private readonly Stack<DependencyRegistration> recursionDefender = new Stack<DependencyRegistration>();

        private ILogger log;

        public ResolveContext(DependencyRegistrationCollection registrations, ILogger log)
        {
            this.Registrations = registrations;
            this.log = log;
            this.Builder = new ObjectBuilder(this, log);
        }

        public ObjectBuilder Builder { get; private set; }

        public DependencyRegistrationCollection Registrations { get; set; }

        protected InternalDependencyResolver Resolver { get; set; }

        public bool CanResolve(DependencyRegistration registration)
        {
            return !this.recursionDefender.Contains(registration);
        }

        public object Resolve(Type serviceType)
        {
            return this.Resolve(this.Registrations.GetRegistrationForService(serviceType));
        }

        public object Resolve(DependencyRegistration registration)
        {
            if (this.recursionDefender.Contains(registration))
            {
                throw new InvalidOperationException("Recursive dependencies are not allowed.");
            }

            try
            {
                this.recursionDefender.Push(registration);

                return registration.LifetimeManager.Resolve(this, registration);
            }
            finally
            {
                this.recursionDefender.Pop();
            }
        }
    }
}