namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;noscript&gt; element.
    /// </summary>
    public interface INoScriptElement : IAttributesCommon,
                                        IContentSetBlock,
                                        IContentSetInline,
                                        IContentModel<INoScriptElement, IContentSetHeading>,
                                        IContentModel<INoScriptElement, IContentSetList>,
                                        IContentModel<INoScriptElement, IContentSetBlock>
    {
    }
}