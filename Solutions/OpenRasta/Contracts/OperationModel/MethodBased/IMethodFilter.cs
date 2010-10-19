namespace OpenRasta.Contracts.OperationModel.MethodBased
{
    using System.Collections.Generic;

    using OpenRasta.Contracts.TypeSystem;

    public interface IMethodFilter
    {
        IEnumerable<IMethod> Filter(IEnumerable<IMethod> methods);
    }
}