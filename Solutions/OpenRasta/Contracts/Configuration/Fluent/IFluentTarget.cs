namespace OpenRasta.Contracts.Configuration.Fluent
{
    using OpenRasta.Configuration.MetaModel;
    using OpenRasta.Contracts.TypeSystem;

    public interface IFluentTarget : INoIzObject
    {
        IMetaModelRepository Repository { get; }

        ITypeSystem TypeSystem { get; }
    }
}