namespace OpenRasta.TypeSystem.ReflectionBased
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public abstract class ReflectionBasedMember<T> : IMember where T : IMemberBuilder
    {
        private readonly Dictionary<string, IProperty> propertiesCachedByPath = new Dictionary<string, IProperty>(StringComparer.OrdinalIgnoreCase);
        private readonly object syncRoot = new object();

        private IType memberType;
        private ILookup<string, IMethod> methodsCache;

        protected ReflectionBasedMember(ITypeSystem typeSystem, Type targetType)
        {
            this.TypeSystem = typeSystem;
            this.SurrogateProvider = typeSystem.SurrogateProvider;
            this.PathManager = typeSystem.PathManager;
            this.TargetType = targetType;
        }

        public virtual bool IsEnumerable
        {
            get { return this.TargetType.IsArray || (this.TargetType.Implements(typeof(IEnumerable<>)) && !this.TargetType.Implements(typeof(IDictionary<,>))); }
        }

        public virtual string Name
        {
            get { return this.TargetType.Name; }
        }

        public Type StaticType
        {
            get { return this.TargetType; }
        }

        public IPathManager PathManager { get; set; }

        public ISurrogateProvider SurrogateProvider { get; set; }

        public Type TargetType { get; set; }

        public virtual IType Type
        {
            get
            {
                if (this.memberType == null)
                {
                    lock (this.syncRoot)
                    {
                        Thread.MemoryBarrier();

                        if (this.memberType == null)
                        {
                            this.memberType = this.TypeSystem.FromClr(this.TargetType);
                        }
                    }
                }

                return this.memberType;
            }
        }

        public virtual string TypeName
        {
            get { return this.TargetType.Name; }
        }

        public ITypeSystem TypeSystem { get; set; }

        public TAttribute FindAttribute<TAttribute>() where TAttribute : class
        {
            return FindAttributes<TAttribute>().FirstOrDefault();
        }

        public IEnumerable<TAttribute> FindAttributes<TAttribute>() where TAttribute : class
        {
            return Attribute.GetCustomAttributes(this.TargetType, true).OfType<TAttribute>();
        }

        public virtual bool CanSetValue(object value)
        {
            return
                (this.TargetType.IsValueType && value != null && this.TargetType.IsAssignableFrom(value.GetType()))
                || (!this.TargetType.IsValueType && (value == null || this.TargetType.IsAssignableFrom(value.GetType())));
        }

        public virtual IProperty GetIndexer(string indexerParameter)
        {
            var indexer = this.TargetType.FindIndexers(1).FindIndexer(indexerParameter);

            return indexer != null ? this.SurrogateProperty(new ReflectionBasedProperty(this.TypeSystem, this, indexer.Value.Key, indexer.Value.Value)) : null;
        }

        public IMethod GetMethod(string methodName)
        {
            this.VerifyMethodsInitialized();

            return this.methodsCache.Contains(methodName) ? this.methodsCache[methodName].FirstOrDefault() : null;
        }

        public IList<IMethod> GetMethods()
        {
            this.VerifyMethodsInitialized();

            return this.methodsCache.SelectMany(x => x).ToList().AsReadOnly();
        }

        public virtual IProperty GetProperty(string propertyName)
        {
            lock (this.syncRoot)
            {
                if (this.propertiesCachedByPath.ContainsKey(propertyName))
                {
                    return this.propertiesCachedByPath[propertyName];
                }

                var pi = this.TargetType.FindPropertyCaseInvariant(propertyName);

                if (pi == null)
                {
                    return null;
                }

                var pa = this.SurrogateProperty(new ReflectionBasedProperty(this.TypeSystem, this, pi, null));
                this.propertiesCachedByPath.Add(propertyName, pa);
                
                return pa;
            }
        }

        private IProperty SurrogateProperty(IProperty property)
        {
            if (this.TypeSystem.SurrogateProvider == null)
            {
                return property;
            }

            return this.TypeSystem.SurrogateProvider.FindSurrogate(property);
        }

        private void VerifyMethodsInitialized()
        {
            if (this.methodsCache == null)
            {
                lock (this.syncRoot)
                {
                    Thread.MemoryBarrier();
                    
                    if (this.methodsCache == null)
                    {
                        var allProperties = this.TargetType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        this.methodsCache = (from method in this.TargetType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                                             where !allProperties.Any(x => x.GetGetMethod() == method || x.GetSetMethod() == method)
                                             select new ReflectionBasedMethod(this.TypeSystem.FromClr(method.DeclaringType), method) as IMethod)
                                             .ToLookup(x => x.Name, StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
        }
    }
}