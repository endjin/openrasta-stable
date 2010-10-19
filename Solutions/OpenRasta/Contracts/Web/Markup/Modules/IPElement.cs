namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;p&gt; element.
    /// </summary>
    public interface IPElement : IAttributesCommon,
                                 IContentSetBlock,
                                 IContentModel<IPElement, string>,
                                 IContentModel<IPElement, IContentSetInline>
    {
    }
}