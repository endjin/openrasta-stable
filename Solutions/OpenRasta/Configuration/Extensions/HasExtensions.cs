namespace OpenRasta.Configuration.Extensions
{
    #region Using Directives

    using System;

    using OpenRasta.Codecs.Framework;
    using OpenRasta.Configuration.Fluent;
    using OpenRasta.Configuration.MetaModel;
    using OpenRasta.Contracts.Configuration.Fluent;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public static class HasExtensions
    {
        public static IResourceDefinition ResourcesNamed(this IHas has, string name)
        {
            return has.ResourcesWithKey(name);
        }

        public static IResourceDefinition ResourcesOfType<T>(this IHas has)
        {
            return has.ResourcesWithKey(typeof(T));
        }

        public static IResourceDefinition ResourcesOfType(this IHas has, Type clrType)
        {
            return has.ResourcesWithKey(clrType);
        }

        public static IResourceDefinition ResourcesOfType(this IHas has, IType type)
        {
            return has.ResourcesWithKey(type);
        }

        public static IResourceDefinition ResourcesWithKey(this IHas has, object resourceKey)
        {
            if (has == null)
            {
                throw new ArgumentNullException("has");
            }

            if (resourceKey == null)
            {
                throw new ArgumentNullException("resourceKey");
            }

            var resourceKeyAsType = resourceKey as Type;
            
            bool strictRegistration = false;
            
            if (resourceKeyAsType != null && CodecRegistration.IsStrictRegistration(resourceKeyAsType))
            {
                resourceKey = CodecRegistration.GetStrictType(resourceKeyAsType);
                strictRegistration = true;
            }
            
            var registration = new ResourceModel
            {
                ResourceKey = resourceKey, 
                IsStrictRegistration = strictRegistration
            };

            var hasBuilder = (IFluentTarget)has;
            hasBuilder.Repository.ResourceRegistrations.Add(registration);
            
            return new ResourceDefinition(hasBuilder.TypeSystem, registration);
        }
    }
}