namespace OpenRasta.Contracts.Pipeline
{
    public interface IContextStore
    {
        object this[string key]
        {
            get; set;
        }
    }
}