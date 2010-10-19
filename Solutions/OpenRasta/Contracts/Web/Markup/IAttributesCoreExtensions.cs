namespace OpenRasta.Contracts.Web.Markup
{
    #region Using Directives

    using System.Xml;

    using OpenRasta.Contracts.Web.Markup.Attributes;

    #endregion

    public static class IAttributesCoreExtensions
    {
        public static T Class<T>(this T element, string className) where T : IAttributesCore
        {
            element.Class.Add(className);
            
            return element;
        }

        public static T XmlSpace<T>(this T element, XmlSpace xmlSpace) where T : IAttributesCore
        {
            element.XmlSpace = xmlSpace;

            return element;
        }
    }
}