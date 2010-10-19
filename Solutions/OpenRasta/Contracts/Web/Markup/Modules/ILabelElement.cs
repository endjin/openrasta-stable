namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the &lt;label&gt; element.
    /// </summary>
    public interface ILabelElement : IAttributesCommon,
                                     IAccessKeyAttribute,
                                     IContentSetFormctrl,
                                     IContentModel<ILabelElement, IContentSetInline>,
                                     IContentModel<ILabelElement, string>
    {
        [IDREF]
        string For { get; set; }
    }
}