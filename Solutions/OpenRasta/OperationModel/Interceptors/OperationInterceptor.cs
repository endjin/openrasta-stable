namespace OpenRasta.OperationModel.Interceptors
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.OperationModel.Interceptors;

    #endregion

    public abstract class OperationInterceptor : IOperationInterceptor
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