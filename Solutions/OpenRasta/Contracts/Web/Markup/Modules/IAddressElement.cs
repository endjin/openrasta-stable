// Text module and presentation module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_textmodule
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_presentationmodule
namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;address&gt; element.
    /// </summary>
    public interface IAddressElement : IAttributesCommon,
                                       IContentSetBlock,
                                       IContentModel<IAddressElement, string>,
                                       IContentModel<IAddressElement, IContentSetInline>
    {
    }
}