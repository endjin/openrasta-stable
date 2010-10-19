// Base module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_basemodule

namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Web.Markup.Attributes;

    #endregion

    /// <summary>
    /// Represents the &lt;base&gt; element.
    /// </summary>
    public interface IBaseElement : IElement, IHrefAttribute, IIDAttribute
    {
    }
}