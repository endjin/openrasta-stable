namespace OpenRasta.Hosting.InMemory
{
    using OpenRasta.Collections;
    using OpenRasta.Contracts.Pipeline;

    public class InMemoryContextStore : NullBehaviorDictionary<string, object>, IContextStore
    {
    }
}