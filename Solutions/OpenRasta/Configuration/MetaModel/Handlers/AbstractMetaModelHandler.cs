namespace OpenRasta.Configuration.MetaModel.Handlers
{
    using OpenRasta.Contracts.Configuration.MetaModel;
    using OpenRasta.Contracts.Configuration.MetaModel.Handlers;

    public abstract class AbstractMetaModelHandler : IMetaModelHandler
    {
        public virtual void PreProcess(IMetaModelRepository repository)
        {
        }

        public virtual void Process(IMetaModelRepository repository)
        {
        }
    }
}