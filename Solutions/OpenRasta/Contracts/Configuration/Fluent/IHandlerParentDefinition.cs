namespace OpenRasta.Contracts.Configuration.Fluent
{
    using System;

    using OpenRasta.Contracts.TypeSystem;

    public interface IHandlerParentDefinition : INoIzObject
    {
        IHandlerForResourceWithUriDefinition HandledBy<T>();

        IHandlerForResourceWithUriDefinition HandledBy(Type type);

        IHandlerForResourceWithUriDefinition HandledBy(IType type);
    }
}