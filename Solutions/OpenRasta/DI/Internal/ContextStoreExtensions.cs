namespace OpenRasta.DI.Internal
{
    using System.Collections.Generic;

    using OpenRasta.Pipeline;

    public static class ContextStoreExtensions
    {
        private const string CtxInstancesKey = "__OR_CTX_INSTANCES_KEY";

        public static void Destruct(this IContextStore store)
        {
            foreach (var dep in store.GetContextInstances())
            {
                if (dep.Cleaner != null)
                {
                    dep.Cleaner.Destruct(dep.Key, dep.Instance);
                }
            }

            store.GetContextInstances().Clear();
        }

        public static IList<ContextStoreDependency> GetContextInstances(this IContextStore store)
        {
            return (IList<ContextStoreDependency>)
                   (store[CtxInstancesKey] ?? (store[CtxInstancesKey] = new List<ContextStoreDependency>()));
        }
    }
}