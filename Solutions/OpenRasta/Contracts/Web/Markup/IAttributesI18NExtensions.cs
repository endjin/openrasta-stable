namespace OpenRasta.Contracts.Web.Markup
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup;

    public static class IAttributesI18NExtensions
    {
        public static T Dir<T>(this T element, Direction textDirection) where T : IAttributesI18N
        {
            element.Dir = textDirection;

            return element;
        }

        public static T XmlLang<T>(this T element, string language) where T : IAttributesI18N
        {
            element.XmlLang = language;

            return element;
        }
    }
}