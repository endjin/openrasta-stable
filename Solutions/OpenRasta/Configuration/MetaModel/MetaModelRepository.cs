namespace OpenRasta.Configuration.MetaModel
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Configuration.MetaModel.Handlers;
    using OpenRasta.Contracts.DI;
    using OpenRasta.DI;

    public class MetaModelRepository : IMetaModelRepository
    {
        private readonly IMetaModelHandler[] handlers;

        // TODO: Remove when impelemntation of array injection in containers is complete
        public MetaModelRepository(IDependencyResolver resolver) : this(resolver.ResolveAll<IMetaModelHandler>().ToArray())
        {
        }

        public MetaModelRepository(IMetaModelHandler[] handlers)
        {
            this.handlers = handlers;
            this.ResourceRegistrations = new List<ResourceModel>();
            this.CustomRegistrations = new ArrayList();
        }

        public IList CustomRegistrations { get; set; }

        public IList<ResourceModel> ResourceRegistrations { get; set; }

        public void Process()
        {
            foreach (var handler in this.handlers)
            {
                handler.PreProcess(this);
            }

            foreach (var handler in this.handlers)
            {
                handler.Process(this);
            }
        }
    }
}