namespace OpenRasta.TypeSystem
{
    using OpenRasta.Contracts.TypeSystem;

    public static class TypeSystemExtensions
    {
        public static IType FromClr<T>(this ITypeSystem typeSystem)
        {
            return typeSystem.FromClr(typeof(T));
        }
    }
}