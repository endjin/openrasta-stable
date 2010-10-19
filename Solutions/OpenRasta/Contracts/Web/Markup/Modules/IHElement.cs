namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;h1&gt;, &lt;h2&gt;, &lt;h3&gt;, &lt;h4&gt;, &lt;h5&gt; and &lt;h6&gt; elements.
    /// </summary>
    public interface IHElement : IContentSetHeading,
                                 IAttributesCommon,
                                 IContentModel<IHElement, string>,
                                 IContentModel<IHElement, IContentSetInline>
    {
    }
}