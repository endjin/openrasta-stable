namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    #endregion

    /// <summary>
    /// Represents the &lt;option&gt; element.
    /// </summary>
    public interface IOptionElement : IAttributesCommon,
                                      IDisabledAttribute,
                                      ILabelAttribute,
                                      IValueAttribute,
                                      IContentModel<IOptionElement, string>
    {
        [Boolean]
        bool Selected { get; set; }
    }
}