namespace OpenRasta.Contracts.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup;

    /// <summary>
    /// <para>Represens the set of Flow elements</para>
    /// <para>
    /// Block elements: &lt;address&gt;, &lt;blockquote&gt;, &lt;div&gt;, &lt;p&gt;, &lt;pre&gt;, &lt;script&gt;, &lt;noscript&gt;</para>
    /// <para>
    /// Form elements: &lt;form&gt; and &lt;fieldset&gt;
    /// </para>
    /// <para>
    /// Table elements: &lt;table&gt;
    /// </para>
    /// <para>
    /// List elements: &lt;dl&gt;, &lt;ol&gt; and &lt;ul&gt;
    /// </para>
    /// <para>
    /// Header elements: &lt;h1&gt;, &lt;h2&gt;, &lt;h3&gt;, &lt;h4&gt;, &lt;h5&gt;, &lt;h6&gt;
    /// </para>
    /// <para>
    /// Inline Elements: &lt;a&gt;, &lt;del&gt;, &lt;ins&gt;, &lt;iframe&gt;, &lt;script&gt;, &lt;noscript&gt; and &lt;object&gt;
    /// </para>
    /// <para>
    /// Empty elements: &lt;hr&gt;, &lt;br&gt; and &lt;img&gt;</para>
    /// <para>
    /// Text elements: &lt;abbr&gt;, &lt;acronym&gt;, &lt;b&gt;, &lt;big&gt;, &lt;cite&gt;, &lt;code&gt;, &lt;dfn&gt;, &lt;em&gt;, &lt;i&gt;, &lt;kbd&gt;, &lt;samp&gt;, &lt;small&gt;, &lt;span&gt;, &lt;strong&gt;, &lt;sub&gt;, &lt;sup&gt;, &lt;tt&gt; and &lt;var&gt;
    /// </para>
    /// </summary>
    public interface IContentSetFlow : IElement
    {
    }
}