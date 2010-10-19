namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;fieldset&gt; element.
    /// </summary>
    public interface IFieldsetElement : IAttributesCommon,
                                        IContentSetForm,
                                        IContentModel<IFieldsetElement, string>,
                                        IContentModel<IFieldsetElement, ILegendElement>,
                                        IContentModel<IFieldsetElement, IContentSetFlow>
    {
    }
}