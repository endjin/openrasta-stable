namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;ul&gt; and &lt;ol&gt; elements
    /// </summary>
    public interface IListElement : IAttributesCommon,
                                    IContentSetList,
                                    IContentModel<IListElement, ILiElement>
    {
    }
}