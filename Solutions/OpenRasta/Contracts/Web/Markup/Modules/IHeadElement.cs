namespace OpenRasta.Web.Markup.Modules
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the &lt;head&gt; element.
    /// </summary>
    public interface IHeadElement : IIDAttribute,
                                    IAttributesI18N,
                                    IContentModel<IHeadElement, ITitleElement>,
                                    IContentModel<IHeadElement, ILinkElement>,
                                    IContentModel<IHeadElement, IScriptElement>,
                                    IContentModel<IHeadElement, IStyleElement>,
                                    IContentModel<IHeadElement, IBaseElement>,
                                    IContentModel<IHeadElement, IMetaElement>
    {
        [URIs]
        IList<Uri> Profile { get; set; }
    }
}