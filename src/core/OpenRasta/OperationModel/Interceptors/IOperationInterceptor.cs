namespace OpenRasta.OperationModel.Interceptors
{
    using System;
    using System.Collections.Generic;

    public interface IOperationInterceptor
    {
        bool BeforeExecute(IOperation operation);

        Func<IEnumerable<OutputMember>> RewriteOperation(Func<IEnumerable<OutputMember>> operationBuilder);

        bool AfterExecute(IOperation operation, IEnumerable<OutputMember> outputMembers);
    }
}