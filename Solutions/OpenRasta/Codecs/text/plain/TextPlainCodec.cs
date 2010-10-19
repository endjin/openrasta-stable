namespace OpenRasta.Codecs.Text.plain
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Web;

    #endregion

    // see rfc2616 for text/plain definition.
    // We completely ignore the Accept-Charset for now.
    [MediaType("text/plain;q=0.5")]
    [SupportedType(typeof(string))]
    public class TextPlainCodec : IMediaTypeWriter, IMediaTypeReader
    {
        private const string EncodingIso88591 = "ISO-8859-1";

        private readonly Dictionary<IHttpEntity, string> values = new Dictionary<IHttpEntity, string>();

        public object Configuration { get; set; }

        public object ReadFrom(IHttpEntity request, IType destinationType, string destinationParameterName)
        {
            if (request.ContentLength == 0)
            {
                return string.Empty;
            }

            if (!this.values.ContainsKey(request))
            {
                var encoding = DetectTextEncoding(request);

                string result = new StreamReader(request.Stream, encoding).ReadToEnd();
                this.values.Add(request, result);
            }

            return this.values[request];
        }

        public void WriteTo(object entity, IHttpEntity response, string[] parameters)
        {
            var entityString = entity.ToString();
            
            var encodedText = Encoding.GetEncoding(EncodingIso88591).GetBytes(entityString);
            response.ContentType = new MediaType("text/plain;charset=ISO-8859-1");
            response.ContentLength = encodedText.Length;
            response.Stream.Write(encodedText, 0, encodedText.Length);
        }

        private static Encoding DetectTextEncoding(IHttpEntity request)
        {
            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding(request.ContentType.CharSet);
            }
            catch
            {
                return Encoding.UTF8;

                // we always default to UTF8 and try to decode.
                // Reason is that the text codec is used by multipart, and browsers send UTF-8 by default.
                // TODO: Log an error
            }

            return encoding;
        }
    }
}