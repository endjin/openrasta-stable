namespace OpenRasta.Contracts.Web.Markup.Modules
{
    /// <summary>
    /// <para>Represents the set of Inline elements.</para>
    /// <para>
    /// Inline Elements: &lt;a&gt;, &lt;del&gt;, &lt;ins&gt;, &lt;iframe&gt;, &lt;script&gt;, &lt;noscript&gt; and &lt;object&gt;
    /// </para>
    /// <para>
    /// Empty elements: &lt;hr&gt;, &lt;br&gt; and &lt;img&gt;</para>
    /// <para>
    /// Text elements: &lt;abbr&gt;, &lt;acronym&gt;, &lt;b&gt;, &lt;big&gt;, &lt;cite&gt;, &lt;code&gt;, &lt;dfn&gt;, &lt;em&gt;, &lt;i&gt;, &lt;kbd&gt;, &lt;samp&gt;, &lt;small&gt;, &lt;span&gt;, &lt;strong&gt;, &lt;sub&gt;, &lt;sup&gt;, &lt;tt&gt; and &lt;var&gt;
    /// </para>
    /// </summary>
    public interface IContentSetInline : IContentSetFlow
    {
    }
}