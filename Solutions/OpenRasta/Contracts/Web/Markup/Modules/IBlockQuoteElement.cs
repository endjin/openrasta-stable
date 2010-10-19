namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;

    /// <summary>
    /// Represents the &lt;blockquote&gt; element.
    /// </summary>
    public interface IBlockQuoteElement : IAttributesCommon,
                                          ICiteAttribute,
                                          IContentSetBlock,
                                          IContentModel<IBlockQuoteElement, IContentSetHeading>,
                                          IContentModel<IBlockQuoteElement, IContentSetBlock>,
                                          IContentModel<IBlockQuoteElement, IContentSetList>
    {
    }
}