namespace OpenRasta.Codecs
{
    using OpenRasta.Contracts.Codecs;

    public abstract class Codec : ICodec
    {
        public object Configuration { get; set; }
    }
}