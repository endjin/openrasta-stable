namespace OpenRasta.Web.Markup.Modules
{
    using System.Collections.Generic;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public interface ICellElementBase : IAttributesCommon,
                                        IAlignAttribute,
                                        ICharAttribute,
                                        IValignAttribute
    {
        [Text]
        string Abbr { get; set; }
        [CDATA]
        string Axis { get; set; }

        [Number(DefaultValue = "1")]
        int? ColSpan { get; set; }

        [Number(DefaultValue = "1")]
        int? RowSpan { get; set; }

        [IDREFS]
        IList<string> Headers { get; set; }

        [ScopeAttribute]
        Scope Scope { get; set; }
    }
}