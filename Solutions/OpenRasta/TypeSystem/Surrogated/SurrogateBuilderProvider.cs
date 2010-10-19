namespace OpenRasta.TypeSystem.Surrogated
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.TypeSystem.Surrogates;
    using OpenRasta.DI;

    public class SurrogateBuilderProvider : ISurrogateProvider
    {
        private readonly ISurrogateBuilder[] builders;
        private static readonly Dictionary<IType, IType> TypeCache = new Dictionary<IType, IType>();
        private static readonly Dictionary<IProperty, IProperty> PropCache = new Dictionary<IProperty, IProperty>();
        
        // HACK: Waiting to push Func<IEnumerable<T>> resolution
        // in container. Remove when done.
        public SurrogateBuilderProvider(IDependencyResolver resolver)
            : this(resolver.ResolveAll<ISurrogateBuilder>().ToArray())
        {
        }

        public SurrogateBuilderProvider(ISurrogateBuilder[] builders)
        {
            if (builders == null)
            {
                throw new ArgumentNullException("builders");
            }

            this.builders = builders;
        }

        public T FindSurrogate<T>(T member) where T : IMember
        {
            var t = member as IType;
            
            if (t != null)
            {
                return (T)this.FindTypeSurrogate(t);
            }

            var p = member as IProperty;
            
            if (p != null)
            {
                return (T)this.FindPropertySurrogate(p);
            }

            return member;
        }

        private IProperty FindPropertySurrogate(IProperty property)
        {
            return Cached(
                PropCache,
                property,
                p =>
                {
                    var alienTypes = this.builders.Where(x => x.CanCreateFor(p)).Select(x => x.Create(p)).ToList();
                    return alienTypes.Count > 0 ? new PropertyWithSurrogates(property, alienTypes) : property;
                });
        }

        private IType FindTypeSurrogate(IType type)
        {
            return Cached(
                TypeCache,
                type,
                t =>
                {
                    var surrogates = this.builders.Where(x => x.CanCreateFor(t)).Select(x => x.Create(t)).ToList();
                    return surrogates.Count > 0 ? new TypeWithSurrogates(type, surrogates) : type;
                });
        }

        private T Cached<T>(Dictionary<T, T> cache, T value, Func<T, T> createCached)
        {
            T cachedValue;
            
            if (cache.TryGetValue(value, out cachedValue))
            {
                return cachedValue;
            }

            lock (cache)
            {
                cachedValue = createCached(value);
                cache.Add(value, cachedValue);
                
                return cachedValue;
            }
        }
    }
}