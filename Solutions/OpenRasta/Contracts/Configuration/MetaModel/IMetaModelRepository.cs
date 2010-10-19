namespace OpenRasta.Contracts.Configuration.MetaModel
{
    #region Using Directives

    using System.Collections;
    using System.Collections.Generic;

    using OpenRasta.Configuration.MetaModel;

    #endregion

    public interface IMetaModelRepository
    {
        List<ResourceModel> ResourceRegistrations { get; set; }

        IList CustomRegistrations { get; set; }

        void Process();
    }
}