namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;

    /// <summary>
    /// Represents the &lt;legend&gt; element.
    /// </summary>
    public interface ILegendElement : IAttributesCommon,
                                      IAccessKeyAttribute,
                                      IContentModel<ILegendElement, string>,
                                      IContentModel<ILegendElement, IContentSetInline>
    {
    }
}