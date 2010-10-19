namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;

    #endregion

    /// <summary>
    /// Represents the &lt;optgroup&gt; element.
    /// </summary>
    public interface IOptgroupElement : IAttributesCommon,
                                        IDisabledAttribute,
                                        ILabelAttribute,
                                        IContentModel<IOptgroupElement, IOptionElement>
    {
    }
}