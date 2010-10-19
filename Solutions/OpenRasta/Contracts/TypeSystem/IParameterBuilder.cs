namespace OpenRasta.Contracts.TypeSystem
{
    public interface IParameterBuilder : ITypeBuilder
    {
        IParameter Parameter { get; set; }
    }
}