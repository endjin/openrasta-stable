namespace OpenRasta.TypeSystem
{
    using OpenRasta.TypeSystem.ReflectionBased;
    using OpenRasta.TypeSystem.Surrogated;
    using OpenRasta.TypeSystem.Surrogates;
    using OpenRasta.TypeSystem.Surrogates.Static;

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