namespace OpenRasta.Contracts.Handlers
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public interface IHandlerRepository
    {
        void AddResourceHandler(object resourceKey, IType handlerType);

        void Clear();

        IEnumerable<IType> GetHandlerTypesFor(object resourceKey);
    }
}