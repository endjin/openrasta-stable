namespace OpenRasta.Contracts.Web.Markup
{
    using System;

    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Contracts.Web.Markup.Rendering;
    using OpenRasta.Web.Markup.Rendering;

    public static class IXhtmlAnchorSiteExtensions
    {
        public static IDisposable scope(this IXhtmlAnchorSite site, IContentModel element)
        {
            var nodeWriter = new XhtmlNodeWriter();
            nodeWriter.WriteStartTag(site.Xhtml.AmbientWriter, element);
            return new NodeWriterTerminator(nodeWriter, site.Xhtml.AmbientWriter, element);
        }

        private class NodeWriterTerminator : IDisposable
        {
            private readonly XhtmlNodeWriter nodeWriter;
            private readonly IXhtmlWriter writer;
            private readonly IElement element;

            public NodeWriterTerminator(XhtmlNodeWriter nodeWriter, IXhtmlWriter writer, IElement element)
            {
                this.nodeWriter = nodeWriter;
                this.writer = writer;
                this.element = element;
            }

            public void Dispose()
            {
                this.nodeWriter.WriteEndTag(this.writer, this.element);
            }
        }
    }
}