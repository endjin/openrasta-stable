// The Metainformation module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_iframemodule
namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the &lt;meta&gt; element.
    /// </summary>
    public interface IMetaElement : IElement, IAttributesI18N, IIDAttribute
    {
        [CDATA]
        string Scheme { get; set; }

        [CDATA]
        string Content { get; set; }

        [NMTOKEN("http-equiv")]
        string HttpEquiv { get; set; }

        // we don't use the INameAttribute because it's defined as a CDATA for forms
        // and on meta <meta> it's defined as an NMTOKEN...
        [NMTOKEN]
        string Name { get; set; }
    }
}