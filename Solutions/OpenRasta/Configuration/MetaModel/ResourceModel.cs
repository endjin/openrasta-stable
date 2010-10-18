namespace OpenRasta.Configuration.MetaModel
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.TypeSystem;

    public class ResourceModel
    {
        public ResourceModel()
        {
            this.Uris = new List<UriModel>();
            this.Handlers = new List<IType>();
            this.Codecs = new List<CodecModel>();
        }

        public IList<CodecModel> Codecs { get; private set; }

        public IList<IType> Handlers { get; private set; }

        public bool IsStrictRegistration { get; set; }

        public object ResourceKey { get; set; }

        public IList<UriModel> Uris { get; private set; }

        public override string ToString()
        {
            return string.Format(
                "ResourceKey: {0}, Uris: {1}, Handlers: {2}, Codecs: {3}",
                this.ResourceKey,
                this.Uris.Aggregate(string.Empty, (str, reg) => str + reg + ";"), 
                this.Handlers.Aggregate(string.Empty, (str, reg) => str + reg + ";"), 
                this.Codecs.Aggregate(string.Empty, (str, reg) => str + reg + ";"));
        }
    }
}