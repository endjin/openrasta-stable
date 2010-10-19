namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;dt&gt; element
    /// </summary>
    public interface IDtElement : IAttributesCommon,
                                  IContentModel<IDtElement, string>,
                                  IContentModel<IDtElement, IContentSetInline>
    {
    }
}