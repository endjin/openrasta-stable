namespace OpenRasta.Contracts.Handlers
{
    using System.Collections.Generic;

    using OpenRasta.Contracts.TypeSystem;

    public interface IHandlerRepository
    {
        void AddResourceHandler(object resourceKey, IType handlerType);

        void Clear();

        IEnumerable<IType> GetHandlerTypesFor(object resourceKey);
    }
}