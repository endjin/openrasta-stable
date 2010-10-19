namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public interface ICharAttribute
    {
        [Character]
        char? Char { get; set; }
        [Length]
        string CharOff { get; set; }
    }
}