namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    /// <summary>
    /// Represents the input tags checkbox and radio
    /// </summary>
    public interface IInputCheckedElement : IInputElement
    {
        [Boolean]
        bool Checked { get; set; }
    }
}