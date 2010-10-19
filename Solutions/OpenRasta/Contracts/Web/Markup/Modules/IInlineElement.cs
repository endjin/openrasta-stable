namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;abbr&gt;, &lt;acronym&gt;, &lt;b&gt;, &lt;big&gt;, &lt;cite&gt;, &lt;code&gt;, &lt;dfn&gt;, &lt;em&gt;, &lt;i&gt;, &lt;kbd&gt;, &lt;samp&gt;, &lt;small&gt;, &lt;span&gt;, &lt;strong&gt;, &lt;sub&gt;, &lt;sup&gt;, &lt;tt&gt; and &lt;var&gt; elements.
    /// </summary>
    public interface IInlineElement : IContentSetInline,
                                      IAttributesCommon,
                                      IContentModel<IInlineElement, string>,
                                      IContentModel<IInlineElement, IContentSetInline>
    {
    }
}