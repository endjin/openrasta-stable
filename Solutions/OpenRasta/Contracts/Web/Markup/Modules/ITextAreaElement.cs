namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    #endregion

    /// <summary>
    /// Represents the &lt;textarea&gt; element.
    /// </summary>
    public interface ITextAreaElement : IAttributesCommon,
                                        IAccessKeyAttribute,
                                        IDisabledAttribute,
                                        INameAttribute,
                                        IReadOnlyAttribute,
                                        ITabIndexAttribute,
                                        IContentSetFormctrl,
                                        IContentModel<ITextAreaElement, string>
    {
        [Number]
        int? Cols { get; set; }

        [Number]
        int? Rows { get; set; }
    }
}