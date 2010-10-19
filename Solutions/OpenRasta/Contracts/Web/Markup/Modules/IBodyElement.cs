namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;body&gt; element.
    /// </summary>
    public interface IBodyElement : IContentModel<IBodyElement, IContentSetHeading>,
                                    IContentModel<IBodyElement, IContentSetBlock>,
                                    IContentModel<IBodyElement, IContentSetList>,
                                    IAttributesCommon
    {
    }
}