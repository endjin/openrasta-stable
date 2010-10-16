namespace OpenRasta.Hosting.InMemory
{
    using OpenRasta.Collections;
    using OpenRasta.Pipeline;

    public class InMemoryContextStore : NullBehaviorDictionary<string, object>, IContextStore
    {
    }
}