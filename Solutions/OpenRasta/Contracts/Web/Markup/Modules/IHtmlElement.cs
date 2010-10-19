// Structure module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_structuremodule
namespace OpenRasta.Web.Markup.Modules
{
    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the &lt;html&gt; element.
    /// </summary>
    public interface IHtmlElement : IContentModel<IHtmlElement, IHeadElement>,
                                    IContentModel<IHtmlElement, IBodyElement>,
                                    IAttributesI18N,
                                    IIDAttribute
    {
        [CDATA]
        string Version { get; set; }

        [URI("http://www.w3.org/1999/xhtml", true)]
        Uri XmlNS { get; set; }
    }
}