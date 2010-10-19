namespace OpenRasta.Contracts.Web.Markup.Attributes
{
    using OpenRasta.Web.Markup;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public interface IAttributesI18N
    {
        [DirectionAttribute]
        Direction Dir { get; set; }
        
        [CDATA("xml:lang")]
        string XmlLang { get; set; }
    }
}