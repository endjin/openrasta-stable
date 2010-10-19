namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public interface IColElementBase : IElement, 
                                       IAttributesCommon,
                                       IAlignAttribute,
                                       ICharAttribute,
                                       IValignAttribute
    {
        [Number(DefaultValue = "1")]
        int? Span { get; set; }
    }
}