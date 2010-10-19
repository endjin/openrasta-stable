namespace OpenRasta.TypeSystem
{
    #region Using Directives

    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.TypeSystem.Surrogates;
    using OpenRasta.TypeSystem.ReflectionBased;
    using OpenRasta.TypeSystem.Surrogated;
    using OpenRasta.TypeSystem.Surrogates;

    #endregion

    public static class TypeSystems
    {
        private static readonly ITypeSystem defaultType = new ReflectionBasedTypeSystem(
            new SurrogateBuilderProvider(
                new ISurrogateBuilder[]
                {
                    new DateTimeSurrogate(), 
                    new ListIndexerSurrogateBuilder(), 
                    new CollectionIndexerSurrogateBuilder()
                }), 
            new PathManager());

        public static ITypeSystem Default
        {
            get { return defaultType; }
        }
    }
}