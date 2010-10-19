namespace OpenRasta.Contracts.Web
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;

    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Exceptions;
    using OpenRasta.Web;

    #endregion

    public interface IHttpEntity
    {
        ICodec Codec { get; set; }

        long? ContentLength { get; set; }

        MediaType ContentType { get; set; }

        object Instance { get; set; }

        Stream Stream { get; }

        HttpHeaderDictionary Headers { get; }

        IList<Error> Errors { get; }
    }
}