namespace OpenRasta.Web.Markup.Modules
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the &lt;object&gt; element
    /// </summary>
    public interface IObjectElement : IAttributesCommon,
                                      INameAttribute,
                                      IWidthHeightAttribute,
                                      ITabIndexAttribute,
                                      ITypeAttribute,
                                      IContentModel<IObjectElement, string>,
                                      IContentModel<IObjectElement, IContentSetFlow>,
                                      IContentModel<IObjectElement, IParamElement>,
                                      IContentSetInline
    {
        [URIs]
        IList<Uri> Archive { get; set; }

        [URI]
        Uri ClassID { get; set; }

        [URI]
        Uri CodeBase { get; set; }

        [ContentType]
        MediaType CodeType { get; set; }

        [URI]
        Uri Data { get; set; }

        [Boolean]
        bool Declare { get; set; }

        [CDATA]
        string StandBy { get; set; }
    }
}