namespace OpenRasta.Contracts.TypeSystem.Surrogated
{
    public interface IHasWrappedMember
    {
        IMember WrappedMember { get; }
    }
}