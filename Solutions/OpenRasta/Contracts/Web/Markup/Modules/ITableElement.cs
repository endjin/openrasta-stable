namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the &lt;table&gt; element.
    /// </summary>
    public interface ITableElement : IAttributesCommon,
                                     IWidthAttribute,
                                     IContentModel<ITableElement, ICaptionElement>,
                                     IContentModel<ITableElement, IColElement>,
                                     IContentModel<ITableElement, IColGroupElement>,
                                     IContentModel<ITableElement, ITHeadElement>,
                                     IContentModel<ITableElement, ITBodyElement>,
                                     IContentModel<ITableElement, ITFootElement>,
                                     IContentSetBlock
    {
        [Pixels]
        int? Border { get; set; }

        [Length]
        string CellPadding { get; set; }
        
        [Length]
        string CellSpacing { get; set; }
        
        [Frame]
        Frame Frame { get; set; }
        
        [Rules]
        Rules Rules { get; set; }
        
        [Text]
        string Summary { get; set; }
    }
}