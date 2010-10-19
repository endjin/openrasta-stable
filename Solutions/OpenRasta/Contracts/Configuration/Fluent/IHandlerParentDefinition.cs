namespace OpenRasta.Contracts.Configuration.Fluent
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.TypeSystem; 

    #endregion;

    public interface IHandlerParentDefinition : INoIzObject
    {
        IHandlerForResourceWithUriDefinition HandledBy<T>();

        IHandlerForResourceWithUriDefinition HandledBy(Type type);

        IHandlerForResourceWithUriDefinition HandledBy(IType type);
    }
}