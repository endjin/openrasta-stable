namespace OpenRasta.Web.Markup.Modules
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the &lt;form&gt; element.
    /// </summary>
    public interface IFormElement : IContentSetForm,
                                    IAttributesCommon,
                                    IAcceptAttribute,
                                    IContentModel<IFormElement, IContentSetHeading>,
                                    IContentModel<IFormElement, IContentSetList>,
                                    IContentModel<IFormElement, IContentSetBlock>,
                                    IContentModel<IFormElement, IFieldsetElement>
    {
        [Charsets("accept-charset")]
        IList<string> AcceptCharset { get; }

        [URI]
        Uri Action { get; set; }

        [CDATA(DefaultValue = "GET")]
        string Method { get; set; }

        [ContentType(DefaultValue = "application/www-url-formencoded")]
        MediaType EncType { get; set; }
    }
}