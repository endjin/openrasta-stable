// The Image module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_imagemodule
namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Elements;

    /// <summary>
    /// Represents the &lt;img&gt; element
    /// </summary>
    public interface IImgElement : IEmptyElement,
                                   IAttributesCommon,
                                   IAltAttribute,
                                   IWidthHeightAttribute,
                                   ILongDescAttribute,
                                   ISrcAttribute
    {
    }
}