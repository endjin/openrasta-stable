namespace OpenRasta.Configuration.MetaModel.Handlers
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Configuration.MetaModel;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public class TypeRewriterMetaModelHandler : AbstractMetaModelHandler
    {
        private readonly ITypeSystem typeSystem;

        public TypeRewriterMetaModelHandler(ITypeSystem typeSystem)
        {
            this.typeSystem = typeSystem;
        }

        public override void PreProcess(IMetaModelRepository repository)
        {
            foreach (var resource in repository.ResourceRegistrations)
            {
                if (resource.ResourceKey is Type)
                {
                    resource.ResourceKey = this.typeSystem.FromClr((Type)resource.ResourceKey);
                }
            }
        }
    }
}