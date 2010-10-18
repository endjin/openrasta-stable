namespace OpenRasta.OperationModel.MethodBased
{
    using System.Collections.Generic;

    using OpenRasta.TypeSystem;

    public interface IMethodFilter
    {
        IEnumerable<IMethod> Filter(IEnumerable<IMethod> methods);
    }
}