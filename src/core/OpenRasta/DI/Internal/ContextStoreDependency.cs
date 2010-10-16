namespace OpenRasta.DI.Internal
{
    using System;

    using OpenRasta.Pipeline;

    public class ContextStoreDependency
    {
        public ContextStoreDependency(string key, object instance, IContextStoreDependencyCleaner cleaner)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            this.Key = key;
            this.Instance = instance;
            this.Cleaner = cleaner;
        }

        public IContextStoreDependencyCleaner Cleaner { get; set; }

        public object Instance { get; set; }

        public string Key { get; set; }
    }
}