namespace OpenRasta.TypeSystem.ReflectionBased
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class ReflectionBasedTypeSystem : ITypeSystem
    {
        private static readonly IDictionary<Type, IType> Cache = new Dictionary<Type, IType>();
        private readonly Stack<Type> recursionDefender = new Stack<Type>();

        public ReflectionBasedTypeSystem()
        {
            this.SurrogateProvider = null;
            this.PathManager = new PathManager();
        }

        public ReflectionBasedTypeSystem(ISurrogateProvider surrogateProvider, IPathManager pathManager)
        {
            this.SurrogateProvider = surrogateProvider;
            this.PathManager = pathManager;
        }

        public IPathManager PathManager { get; private set; }

        public ISurrogateProvider SurrogateProvider { get; private set; }

        public IType FromClr(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            IType result;
            if (!Cache.TryGetValue(type, out result))
            {
                lock (Cache)
                {
                    // if (_recursionDefender.Contains(type))
                    // throw new RecursionException();
                    try
                    {
                        this.recursionDefender.Push(type);
                        Thread.MemoryBarrier();
                        
                        if (!Cache.TryGetValue(type, out result))
                        {
                            var typeAccessor = new ReflectionBasedType(this, type);

                            // write the temporary type in the cache to avoid recursion
                            Cache[type] = typeAccessor;
                            result = this.SurrogateProvider != null
                                         ? (this.SurrogateProvider.FindSurrogate((IType)typeAccessor) ?? typeAccessor)
                                         : typeAccessor;

                            // and update
                            Cache[type] = result;
                        }
                    }
                    finally
                    {
                        this.recursionDefender.Pop();
                    }
                }
            }

            return result;
        }

        public IType FromInstance(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return this.FromClr(instance.GetType());
        }
    }
}