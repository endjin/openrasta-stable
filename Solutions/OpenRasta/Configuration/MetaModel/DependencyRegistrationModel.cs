namespace OpenRasta.Configuration.MetaModel
{
    #region Using Directives

    using System;

    using OpenRasta.DI;

    #endregion

    public class DependencyRegistrationModel
    {
        public DependencyRegistrationModel(Type serviceType, Type concreteType, DependencyLifetime lifetime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (concreteType == null)
            {
                throw new ArgumentNullException("concreteType");
            }
            
            this.ServiceType = serviceType;
            this.ConcreteType = concreteType;
            this.Lifetime = lifetime;
        }

        public Type ConcreteType { get; private set; }

        public DependencyLifetime Lifetime { get; private set; }

        public Type ServiceType { get; private set; }
    }
}