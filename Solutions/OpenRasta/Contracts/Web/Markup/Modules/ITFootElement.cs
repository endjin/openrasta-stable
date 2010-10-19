namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;tfoot&gt; element.
    /// </summary>
    public interface ITFootElement : ITableSectionElementBase,
                                     IContentModel<ITHeadElement, ITrElement>
    {
    }
}