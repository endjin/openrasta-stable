namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;th&gt; element.
    /// </summary>
    public interface IThElement : ICellElementBase,
                                  IContentModel<IThElement, IContentSetFlow>,
                                  IContentModel<IThElement, string>
    {
    }
}