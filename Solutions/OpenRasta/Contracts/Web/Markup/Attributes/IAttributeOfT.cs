namespace OpenRasta.Contracts.Web.Markup.Attributes
{
    public interface IAttribute<T> : IAttribute
    {
        T Value { get; set; }
    }
}