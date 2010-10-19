namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes;

    /// <summary>
    /// Represents the &lt;title&gt; element.
    /// </summary>
    public interface ITitleElement : IIDAttribute,
                                     IAttributesI18N,
                                     IContentModel<ITitleElement, string>
    {
    }
}