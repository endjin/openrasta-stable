namespace OpenRasta.Contracts.Web.Markup.Attributes
{
    public interface IAttribute
    {
        string Name { get; set; }

        string SerializedValue { get; set; }

        string DefaultValue { get; set; }

        bool IsDefault { get; }

        bool RendersOnDefaultValue { get; set; }
    }
}