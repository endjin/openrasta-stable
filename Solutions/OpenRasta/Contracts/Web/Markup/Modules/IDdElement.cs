namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;dd&gt; element
    /// </summary>
    public interface IDdElement : IAttributesCommon,
                                  IContentModel<IDdElement, string>,
                                  IContentModel<IDdElement, IContentSetFlow>
    {
    }
}