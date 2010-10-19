namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;div&gt; element.
    /// </summary>
    public interface IDivElement : IAttributesCommon,
                                   IContentSetBlock,
                                   IContentModel<IDivElement, string>,
                                   IContentModel<IDivElement, IContentSetFlow>
    {
    }
}