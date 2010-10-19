namespace OpenRasta.Web.Markup.Rendering
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Text;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Contracts.Web.Markup.Rendering;
    using OpenRasta.Extensions;

    #endregion

    public class XhtmlTextWriter : IXhtmlWriter, ISupportsTextWriter
    {
        private const string TagAttr = " {0}=\"{1}\"";
        private const string TagEnd = "</{0}>";
        private const string TagStartBegin = "<{0}";
        private const string TagStartEnd = ">";
        private const string TagStartEndFinal = " />";

        public XhtmlTextWriter(TextWriter source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "source is null.");
            }

            TextWriter = source;
        }

        public TextWriter TextWriter
        {
            get; private set;
        }

        public static string HtmlEncode(string source)
        {
            if (source == null)
            {
                return null;
            }

            var builder = new StringBuilder();
            
            foreach (var c in source)
            {
                switch (c)
                {
                    case '"':
                    case '\'':
                    case '&':
                    case '<':
                    case '>':
                        builder.Append("&#").Append((int)c).Append(';');
                        break;
                    default:
                        builder.Append(c);
                        break;
                }
            }

            return builder.ToString();
        }

        public void BeginWriteStartElement(string tagName)
        {
            TextWriter.Write(TagStartBegin.With(tagName));
        }

        public void EndWriteStartElement()
        {
            TextWriter.Write(TagStartEnd);
        }

        public void EndWriteStartElementFinal()
        {
            TextWriter.Write(TagStartEndFinal);
        }

        public void WriteEndElement(string tagName)
        {
            TextWriter.Write(TagEnd.With(tagName));
        }

        public void WriteAttributeString(string key, string value)
        {
            TextWriter.Write(TagAttr.With(key, HtmlEncode(value)));
        }

        public void WriteString(string content)
        {
            TextWriter.Write(HtmlEncode(content));
        }

        public void WriteUnencodedString(string content)
        {
            TextWriter.Write(content);
        }
    }
}