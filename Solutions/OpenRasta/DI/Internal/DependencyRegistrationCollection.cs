namespace OpenRasta.DI.Internal
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Contracts.Pipeline;

    #endregion

    public class DependencyRegistrationCollection : IContextStoreDependencyCleaner
    {
        private readonly Dictionary<Type, List<DependencyRegistration>> registrations = new Dictionary<Type, List<DependencyRegistration>>();

        public IEnumerable<DependencyRegistration> this[Type serviceType]
        {
            get
            {
                lock (this.registrations)
                {
                    return this.GetSvcRegistrations(serviceType);
                }
            }
        }

        public void Add(DependencyRegistration registration)
        {
            registration.LifetimeManager.VerifyRegistration(registration);
            lock (this.registrations)
            {
                this.GetSvcRegistrations(registration.ServiceType).Add(registration);
            }
        }

        public DependencyRegistration GetRegistrationForService(Type type)
        {
            lock (this.registrations)
            {
                return this.GetSvcRegistrations(type).LastOrDefault(x => x.LifetimeManager.IsRegistrationAvailable(x));
            }
        }

        public bool HasRegistrationForService(Type type)
        {
            lock (this.registrations)
            {
                return this.registrations.ContainsKey(type) && this.GetSvcRegistrations(type).ToList().Any(x => x.LifetimeManager.IsRegistrationAvailable(x));
            }
        }

        public void Destruct(string key, object instance)
        {
            lock (this.registrations)
            {
                foreach (var reg in this.registrations)
                {
                    var registrationToRemove = reg.Value.Where(x => x.Key == key).ToList();

                    registrationToRemove.ForEach(x => reg.Value.Remove(x));
                }
            }
        }

        // Not thread safe
        public List<DependencyRegistration> GetSvcRegistrations(Type serviceType)
        {
            List<DependencyRegistration> svcRegistrations;
            if (!this.registrations.TryGetValue(serviceType, out svcRegistrations))
            {
                this.registrations.Add(serviceType, svcRegistrations = new List<DependencyRegistration>());
            }

            return svcRegistrations;
        }
    }
}