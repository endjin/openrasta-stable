namespace OpenRasta.Contracts.Configuration.MetaModel.Handlers
{
    using OpenRasta.Configuration.MetaModel;

    public interface IMetaModelHandler
    {
        void PreProcess(IMetaModelRepository repository);

        void Process(IMetaModelRepository repository);
    }
}