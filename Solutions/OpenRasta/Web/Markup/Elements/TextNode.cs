namespace OpenRasta.Web.Markup.Elements
{
    using OpenRasta.Contracts.Web.Markup;

    public class TextNode : ITextNode
    {
        public TextNode(string content)
        {
            this.Text = content;
        }

        public TextNode()
        {
        }

        public string Text
        {
            get; set;
        }

        public static implicit operator TextNode(string textNodeValue)
        {
            return new TextNode(textNodeValue);
        }
    }
}