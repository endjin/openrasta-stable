namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;td&gt; element.
    /// </summary>
    public interface ITdElement : ICellElementBase,
                                  IContentModel<ITdElement, IContentSetFlow>,
                                  IContentModel<ITdElement, string>
    {
    }
}