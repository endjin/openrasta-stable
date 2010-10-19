namespace OpenRasta.Pipeline.InMemory
{
    #region Using Directives

    using System.IO;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Web;

    #endregion

    public class TextWriterEnabledEntity : HttpEntity, ISupportsTextWriter
    {
        public TextWriterEnabledEntity(TextWriter writer) : base(new HttpHeaderDictionary(), new MemoryStream())
        {
            TextWriter = writer;
        }

        public TextWriter TextWriter { get; set; }
    }
}