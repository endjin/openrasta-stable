namespace OpenRasta.Handlers
{
    using System.Collections.Generic;

    using OpenRasta.TypeSystem;

    public interface IHandlerRepository
    {
        void AddResourceHandler(object resourceKey, IType handlerType);

        void Clear();

        IEnumerable<IType> GetHandlerTypesFor(object resourceKey);
    }
}