// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_hypertextmodule
namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;

    #endregion

    /// <summary>
    /// Represents the &lt;a&gt; element
    /// </summary>
    public interface IAElement : IContentSetInline,
                                 IAttributesCommon,
                                 IAccessKeyAttribute,
                                 ICharSetAttribute,
                                 IHrefAttribute,
                                 ILinkRelationshipAttribute,
                                 ITabIndexAttribute,
                                 ITypeAttribute,
                                 IContentModel<IAElement, IContentSetInline>,
                                 IContentModel<IAElement, string>
    {
    }
}