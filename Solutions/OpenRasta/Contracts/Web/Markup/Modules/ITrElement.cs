namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;tr&gt; element.
    /// </summary>
    public interface ITrElement : ITableSectionElementBase,
                                  IContentModel<ITrElement, ITdElement>,
                                  IContentModel<ITrElement, IThElement>
    {
    }
}