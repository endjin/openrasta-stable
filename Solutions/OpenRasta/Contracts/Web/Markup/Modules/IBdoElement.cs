// Bi-directional text module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_bdomodule

namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    #endregion

    /// <summary>
    /// Represents the &lt;bdo&gt; element.
    /// </summary>
    public interface IBdoElement : IAttributesCore,
                                   IAttributesI18N,
                                   IContentModel<IEditElement, string>,
                                   IContentModel<IEditElement, IContentSetInline>
    {
    }
}