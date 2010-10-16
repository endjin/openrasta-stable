namespace OpenRasta.Configuration.Fluent
{
    using System;

    using OpenRasta.TypeSystem;

    public interface IHandlerParentDefinition : INoIzObject
    {
        IHandlerForResourceWithUriDefinition HandledBy<T>();

        IHandlerForResourceWithUriDefinition HandledBy(Type type);

        IHandlerForResourceWithUriDefinition HandledBy(IType type);
    }
}