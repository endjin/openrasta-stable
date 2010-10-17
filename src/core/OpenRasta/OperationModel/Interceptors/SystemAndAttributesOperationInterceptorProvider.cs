namespace OpenRasta.OperationModel.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.DI;

    public class SystemAndAttributesOperationInterceptorProvider : IOperationInterceptorProvider
    {
        private readonly IOperationInterceptor[] systemInterceptors;

        public SystemAndAttributesOperationInterceptorProvider(IDependencyResolver resolver)
            : this(resolver.ResolveAll<IOperationInterceptor>().ToArray())
        {
        }

        public SystemAndAttributesOperationInterceptorProvider(IOperationInterceptor[] systemInterceptors)
        {
            this.systemInterceptors = systemInterceptors;
        }

        public IEnumerable<IOperationInterceptor> GetInterceptors(IOperation operation)
        {
            return this.systemInterceptors
                .Concat(GetInterceptorAttributes(operation))
                .Concat(GetInterceptorProviderAttributes(operation))
                .ToList();
        }

        private static IEnumerable<IOperationInterceptor> GetInterceptorAttributes(IOperation operation)
        {
            return operation.FindAttributes<IOperationInterceptor>();
        }

        private static IEnumerable<IOperationInterceptor> GetInterceptorProviderAttributes(IOperation operation)
        {
            return operation.FindAttributes<IOperationInterceptorProvider>().SelectMany(x => x.GetInterceptors(operation));
        }
    }
}