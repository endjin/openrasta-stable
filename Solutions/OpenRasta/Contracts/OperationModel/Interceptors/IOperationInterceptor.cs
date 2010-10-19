namespace OpenRasta.Contracts.OperationModel.Interceptors
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.OperationModel;

    #endregion

    public interface IOperationInterceptor
    {
        bool BeforeExecute(IOperation operation);

        Func<IEnumerable<OutputMember>> RewriteOperation(Func<IEnumerable<OutputMember>> operationBuilder);

        bool AfterExecute(IOperation operation, IEnumerable<OutputMember> outputMembers);
    }
}