namespace OpenRasta.DI.Internal
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Pipeline;

    #endregion

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