namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    using System.Xml;

    public class XmlSpaceAttribute : PrimaryTypeAttributeCore
    {
        public XmlSpaceAttribute() : base("xml:space", Factory<XmlSpace>)
        {
        }
    }
}