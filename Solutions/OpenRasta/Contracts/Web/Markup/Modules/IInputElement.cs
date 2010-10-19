namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;

    public interface IInputElement : IAttributesCommon,
                                     IAcceptAttribute,
                                     IAccessKeyAttribute,
                                     IAltAttribute,
                                     IDisabledAttribute,
                                     INameAttribute,
                                     IReadOnlyAttribute,
                                     ITabIndexAttribute,
                                     IValueAttribute,
                                     IContentSetFormctrl
    {
        [InputType]
        InputType Type { get; set; }
    }
}