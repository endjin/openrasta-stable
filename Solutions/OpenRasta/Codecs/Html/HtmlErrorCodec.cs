namespace OpenRasta.Codecs.Html
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;

    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Exceptions;
    using OpenRasta.Web.Markup.Rendering;

    #endregion

    /// <summary>
    /// Codec rendering error messages collected during the processing of a request.
    /// </summary>
    [MediaType("application/xhtml+xml;q=0.9")]
    [MediaType("text/html")]
    [SupportedType(typeof(IList<Error>))]
    public class HtmlErrorCodec : IMediaTypeWriter
    {
        public object Configuration { get; set; }

        public void WriteTo(object entity, IHttpEntity response, string[] paramneters)
        {
            var errors = entity as IList<Error>;
            
            if (errors == null)
            {
                return;
            }

            using (var streamWriter = new StreamWriter(response.Stream))
            {
                var writer = new XhtmlNodeWriter();
                writer.Write(new XhtmlTextWriter(streamWriter), new HtmlErrorPage(errors));
            }
        }
    }
}