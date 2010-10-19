namespace OpenRasta.Contracts.Pipeline
{
    public interface IContextStoreDependencyCleaner
    {
        void Destruct(string key, object instance);
    }
}