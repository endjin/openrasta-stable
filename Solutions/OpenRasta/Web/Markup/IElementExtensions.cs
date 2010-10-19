namespace OpenRasta.Web.Markup
{
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    public static class IElementExtensions
    {
        public static T Attr<T>(this T element, string attributeName, string attributeValue) where T:IElement
        {
            if (element.Attributes[attributeName] == null)
            {
                element.Attributes[attributeName] = new PrimaryTypeAttributeNode<string>(attributeName, true);
            }

            element.Attributes[attributeName].SerializedValue = attributeValue;
            
            return element;
        }
    }
}