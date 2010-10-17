namespace OpenRasta.OperationModel.MethodBased
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Binding;
    using OpenRasta.DI;
    using OpenRasta.TypeSystem;

    public class MethodBasedOperationCreator : IOperationCreator
    {
        private readonly IObjectBinderLocator binderLocator;
        private readonly Func<IEnumerable<IMethod>, IEnumerable<IMethod>> filterMethod;
        private readonly IDependencyResolver resolver;

        // TODO: Remove when support for arrays is added to containers
        public MethodBasedOperationCreator(IDependencyResolver resolver, IObjectBinderLocator binderLocator)
            : this(resolver.ResolveAll<IMethodFilter>().ToArray(), resolver, binderLocator)
        {
        }

        public MethodBasedOperationCreator(IMethodFilter[] filters, IDependencyResolver resolver, IObjectBinderLocator binderLocator)
        {
            this.resolver = resolver;
            this.binderLocator = binderLocator;
            this.filterMethod = this.FilterMethods(filters).Chain();
        }

        public IEnumerable<IOperation> CreateOperations(IEnumerable<IType> handlers)
        {
            return from handler in handlers
                   let sourceMethods = handler.GetMethods()
                   let filteredMethods = this.filterMethod(sourceMethods)
                   from method in filteredMethods
                   select new MethodBasedOperation(this.binderLocator, handler, method) { Resolver = this.resolver } as IOperation;
        }

        private IEnumerable<Func<IEnumerable<IMethod>, IEnumerable<IMethod>>> FilterMethods(IMethodFilter[] filters)
        {
            if (filters == null)
            {
                yield return inMethods => inMethods;
                yield break;
            }

            foreach (var filter in filters)
            {
                yield return filter.Filter;
            }
        }
    }
}