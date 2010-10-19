namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the &lt;input type="text" /&gt; element.
    /// </summary>
    public interface IInputTextElement : IInputElement, ISizeAttribute
    {
        [Number]
        int? MaxLength { get; set; }
    }
}