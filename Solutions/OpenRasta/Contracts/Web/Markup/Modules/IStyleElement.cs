// the Style Sheet and the Style Attribute modules.
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_stylemodule
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_styleattributemodule
namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;

    /// <summary>
    /// Represents the &lt;style&gt; element.
    /// </summary>
    public interface IStyleElement : IAttributesI18N,
                                     IIDAttribute,
                                     IMediaAttribute,
                                     ITitleAttribute,
                                     ITypeAttribute,
                                     IContentModel<IStyleElement, string>
    {
    }
}