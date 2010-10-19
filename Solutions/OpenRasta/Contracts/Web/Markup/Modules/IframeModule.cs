// IFrame module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_iframemodule

namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public enum Scrolling
    {
        Yes,
        No,
        Auto
    }

    /// <summary>
    /// Represents the &lt;iframe&gt; element.
    /// </summary>
    public interface IIFrameElement : IAttributesCore,
                                      IContentSetInline,
                                      ISrcAttribute,
                                      ILongDescAttribute,
                                      IWidthHeightAttribute,
                                      IContentModel<IIFrameElement, string>,
                                      IContentModel<IIFrameElement, IContentSetFlow>
    {
        [DigitBoolean]
        bool FrameBorder { get; set; }

        [Pixels]
        int? MarginWidth { get; set; }

        [Pixels]
        int? MarginHeight { get; set; }

        [Scrolling]
        Scrolling Scrolling { get; set; }
    }

    public class ScrollingAttribute : EnumAttributeCore
    {
        public ScrollingAttribute() : base(Factory<Scrolling>)
        {
        }
    }
}