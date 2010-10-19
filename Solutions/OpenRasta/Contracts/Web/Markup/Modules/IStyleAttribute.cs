namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public interface IStyleAttribute
    {
        [Text]
        string Style { get; set; }
    }
}