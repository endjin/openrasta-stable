namespace OpenRasta.Contracts.Configuration.Fluent
{
    public interface IRepeatableDefinition<TParent> : INoIzObject
    {
        TParent And { get; }
    }
}