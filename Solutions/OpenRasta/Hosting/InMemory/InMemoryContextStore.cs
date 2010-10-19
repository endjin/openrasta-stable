namespace OpenRasta.Hosting.InMemory
{
    using OpenRasta.Collections;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Pipeline;

    public class InMemoryContextStore : NullBehaviorDictionary<string, object>, IContextStore
    {
    }
}