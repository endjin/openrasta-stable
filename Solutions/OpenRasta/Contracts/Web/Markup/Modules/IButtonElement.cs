namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;

    /// <summary>
    /// Represents the &lt;button&gt; element.
    /// </summary>
    public interface IButtonElement : IAttributesCommon,
                                      IAccessKeyAttribute,
                                      IDisabledAttribute,
                                      INameAttribute,
                                      ITabIndexAttribute,
                                      IValueAttribute,
                                      IContentSetFormctrl,
                                      IContentModel<IButtonElement, string>,
                                      IContentModel<IButtonElement, IContentSetList>,
                                      IContentModel<IButtonElement, IContentSetHeading>,
                                      IContentModel<IButtonElement, IContentSetBlock>,
                                      IContentModel<IButtonElement, IContentSetInline>
    {
        [ButtonTypeAttribute]
        ButtonType Type { get; set; }
    }
}