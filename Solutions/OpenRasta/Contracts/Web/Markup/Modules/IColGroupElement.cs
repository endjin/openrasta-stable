namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Defines the &lt;colgroup&gt; element.
    /// </summary>
    public interface IColGroupElement : IColElementBase,
                                        IContentModel<IColGroupElement, IColElement>
    {
    }
}