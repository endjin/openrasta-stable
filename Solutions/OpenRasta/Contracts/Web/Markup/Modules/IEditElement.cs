// Edit module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_editmodule
namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    #endregion

    /// <summary>
    /// Represents the &lt;del&gt; and &lt;ins&gt; elements.
    /// </summary>
    public interface IEditElement : IAttributesCommon,
                                    IContentSetInline,
                                    ICiteAttribute,
                                    IContentModel<IEditElement, string>,
                                    IContentModel<IEditElement, IContentSetFlow>
    {
        [Datetime]
        DateTime? DateTime { get; set; }
    }
}