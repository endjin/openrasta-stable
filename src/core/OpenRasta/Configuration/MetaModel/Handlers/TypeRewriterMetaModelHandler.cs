namespace OpenRasta.Configuration.MetaModel.Handlers
{
    using System;

    using OpenRasta.TypeSystem;

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