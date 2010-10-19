namespace OpenRasta.Contracts.TypeSystem
{
    using OpenRasta.Contracts.DI;

    public interface IResolverAwareType : IType
    {
        object CreateInstance(IDependencyResolver resolver);
    }
}