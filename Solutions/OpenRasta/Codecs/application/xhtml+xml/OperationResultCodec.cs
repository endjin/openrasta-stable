﻿namespace OpenRasta.Codecs
{
    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Web;

    [MediaType("application/xhtml+xml;q=0.9")]
    [MediaType("text/html")]
    [SupportedType(typeof(OperationResult))]
    public class OperationResultCodec : Codec, IMediaTypeWriter
    {
        public void WriteTo(object entity, IHttpEntity response, string[] codecParameters)
        {
        }
    }
}
