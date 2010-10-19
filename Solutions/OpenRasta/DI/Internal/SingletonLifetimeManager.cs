namespace OpenRasta.DI.Internal
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Collections;

    #endregion

    public class SingletonLifetimeManager : DependencyLifetimeManager
    {
        private readonly IDictionary<string, object> instances = new NullBehaviorDictionary<string, object>();

        public SingletonLifetimeManager(InternalDependencyResolver builder)
            : base(builder)
        {
        }

        public override object Resolve(ResolveContext context, DependencyRegistration registration)
        {
            object instance;

            if (!this.instances.TryGetValue(registration.Key, out instance))
            {
                lock (this.instances)
                {
                    if (!this.instances.TryGetValue(registration.Key, out instance))
                    {
                        this.instances.Add(registration.Key, instance = base.Resolve(context, registration));
                    }
                }
            }

            return instance;
        }

        public override void VerifyRegistration(DependencyRegistration registration)
        {
            if (registration.IsInstanceRegistration)
            {
                if (this.instances[registration.Key] != null)
                {
                    throw new InvalidOperationException("Trying to register an instance for a registration that already has one.");
                }

                lock (this.instances)
                {
                    this.instances[registration.Key] = registration.Instance;
                }

                registration.Instance = null;
            }
        }
    }
}