namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;thead&gt; element.
    /// </summary>
    public interface ITHeadElement : ITableSectionElementBase,
                                     IContentModel<ITHeadElement, ITrElement>
    {
    }
}