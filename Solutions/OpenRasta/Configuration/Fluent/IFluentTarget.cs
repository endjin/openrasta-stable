namespace OpenRasta.Configuration.Fluent
{
    using OpenRasta.Configuration.MetaModel;
    using OpenRasta.TypeSystem;

    public interface IFluentTarget : INoIzObject
    {
        IMetaModelRepository Repository { get; }

        ITypeSystem TypeSystem { get; }
    }
}