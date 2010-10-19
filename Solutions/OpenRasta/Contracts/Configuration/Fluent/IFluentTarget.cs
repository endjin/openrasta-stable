namespace OpenRasta.Contracts.Configuration.Fluent
{
    using OpenRasta.Contracts.Configuration.MetaModel;
    using OpenRasta.Contracts.TypeSystem;

    public interface IFluentTarget : INoIzObject
    {
        IMetaModelRepository Repository { get; }

        ITypeSystem TypeSystem { get; }
    }
}