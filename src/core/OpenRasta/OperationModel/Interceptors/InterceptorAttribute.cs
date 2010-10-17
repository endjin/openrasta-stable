namespace OpenRasta.OperationModel.Interceptors
{
    using System;
    using System.Collections.Generic;

    public abstract class InterceptorAttribute : Attribute, IOperationInterceptor
    {
        public virtual bool AfterExecute(IOperation operation, IEnumerable<OutputMember> outputMembers)
        {
            return true;
        }

        public virtual bool BeforeExecute(IOperation operation)
        {
            return true;
        }

        public virtual Func<IEnumerable<OutputMember>> RewriteOperation(Func<IEnumerable<OutputMember>> operationBuilder)
        {
            return operationBuilder;
        }
    }
}