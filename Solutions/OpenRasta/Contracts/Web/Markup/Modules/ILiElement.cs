namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;li&gt; element
    /// </summary>
    public interface ILiElement : IAttributesCommon,
                                  IContentModel<ILiElement, string>,
                                  IContentModel<ILiElement, IContentSetFlow>
    {
    }
}