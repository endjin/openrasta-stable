namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;img&gt;, &lt;hr&gt; and &lt;br&gt; elements
    /// </summary>
    public interface IEmptyElement :
        IContentSetInline,
        IAttributesCore
    {
    }
}