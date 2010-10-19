namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;tbody&gt; element.
    /// </summary>
    public interface ITBodyElement : ITableSectionElementBase,
                                     IContentModel<ITHeadElement, ITrElement>
    {
    }
}