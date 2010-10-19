namespace OpenRasta.Codecs.Application.xml
{
    #region Using Directives

    using System.Xml;

    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;

    #endregion

    public abstract class XmlCodec : IMediaTypeReader, IMediaTypeWriter
    {
        public object Configuration { get; set; }

        protected XmlWriter Writer { get; private set; }

        public abstract object ReadFrom(IHttpEntity request, IType destinationType, string memberName);

        public virtual void WriteTo(object entity, IHttpEntity response, string[] parameters)
        {
            var responseStream = response.Stream;
            var settings = new XmlWriterSettings
                {
                    ConformanceLevel = ConformanceLevel.Document,
                    Indent = true,
                    NewLineOnAttributes = true,
                    OmitXmlDeclaration = false,
                    CloseOutput = true,
                    CheckCharacters = true
                };

            using (this.Writer = XmlWriter.Create(responseStream, settings))
            {
                this.WriteToCore(entity, response);
            }
        }

        protected abstract void WriteToCore(object entity, IHttpEntity response);
    }
}