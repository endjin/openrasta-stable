namespace OpenRasta.Hosting
{
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Pipeline;

    public class AmbientContextStore : IContextStore
    {
        public object this[string key]
        {
            get { return AmbientContext.Current[key]; }
            set { AmbientContext.Current[key] = value; }
        }
    }
}