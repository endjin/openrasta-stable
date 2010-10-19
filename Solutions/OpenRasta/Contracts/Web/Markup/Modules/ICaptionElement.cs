namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;caption&gt; element.
    /// </summary>
    public interface ICaptionElement : IAttributesCommon,
                                       IContentModel<ICaptionElement, IContentSetInline>,
                                       IContentModel<ICaptionElement, string>
    {
    }
}