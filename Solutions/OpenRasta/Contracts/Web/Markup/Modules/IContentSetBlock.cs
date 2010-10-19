namespace OpenRasta.Contracts.Web.Markup.Modules
{
    /// <summary>
    /// <para>Represents the set of Block elements.</para>
    /// <para>
    /// Block elements: &lt;address&gt;, &lt;blockquote&gt;, &lt;div&gt;, &lt;p&gt;, &lt;pre&gt;, &lt;script&gt;, &lt;noscript&gt;
    /// </para>
    /// <para>
    /// Form elements: &lt;form&gt; and &lt;fieldset&gt;
    /// </para>
    /// <para>
    /// Table elements: &lt;table&gt;
    /// </para>
    /// </summary>
    public interface IContentSetBlock : IContentSetFlow
    {
    }
}