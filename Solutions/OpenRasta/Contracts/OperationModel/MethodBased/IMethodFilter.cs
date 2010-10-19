namespace OpenRasta.Contracts.OperationModel.MethodBased
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public interface IMethodFilter
    {
        IEnumerable<IMethod> Filter(IEnumerable<IMethod> methods);
    }
}