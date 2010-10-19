namespace OpenRasta.Configuration.MetaModel
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IMetaModelRepository
    {
        List<ResourceModel> ResourceRegistrations { get; set; }

        IList CustomRegistrations { get; set; }

        void Process();
    }
}