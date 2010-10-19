namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations; 

    #endregion;

    /// <summary>
    /// Represents the &lt;select&gt; element.
    /// </summary>
    public interface ISelectElement : IAttributesCommon,
                                      IDisabledAttribute,
                                      INameAttribute,
                                      ISizeAttribute,
                                      ITabIndexAttribute,
                                      IContentSetFormctrl,
                                      IContentModel<ISelectElement, IOptgroupElement>,
                                      IContentModel<ISelectElement, IOptionElement>
    {
        [Boolean]
        bool? Multiple { get; set; }
    }
}