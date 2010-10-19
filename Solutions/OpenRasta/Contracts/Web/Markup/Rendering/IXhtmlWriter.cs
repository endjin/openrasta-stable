namespace OpenRasta.Contracts.Web.Markup.Rendering
{
    public interface IXhtmlWriter
    {
        void BeginWriteStartElement(string tagName);

        void EndWriteStartElement();

        void EndWriteStartElementFinal();

        void WriteEndElement(string tagName);

        void WriteAttributeString(string attributeName, string attributeValue);

        void WriteString(string content);

        void WriteUnencodedString(string content);
    }
}