// the Scripting Module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_scriptmodule
namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the &lt;script&lt; element.
    /// </summary>
    public interface IScriptElement : IContentSetBlock,
                                      IContentSetInline,
                                      ICharSetAttribute,
                                      IIDAttribute,
                                      ISrcAttribute,
                                      ITypeAttribute,
                                      IContentModel<IScriptElement, string>
    {
        [Boolean]
        bool Defer { get; set; }
    }
}