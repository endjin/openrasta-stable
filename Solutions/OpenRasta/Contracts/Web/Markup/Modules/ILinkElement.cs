// the Link Module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_linkmodule
namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes;

    /// <summary>
    /// Represents the &lt;link&gt; element.
    /// </summary>
    public interface ILinkElement : IElement,
        IAttributesCommon,
        ICharSetAttribute,
        IHrefAttribute,
        IMediaAttribute,
        ILinkRelationshipAttribute,
        ITypeAttribute
    {
    }
}