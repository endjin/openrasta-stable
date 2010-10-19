namespace OpenRasta.Extensions
{
    public static class NullableExtensions
    {
        public static bool IsNullOr<T>(this T? target, T value) where T : struct
        {
            return target == null || target.Value.Equals(value);
        }
    }
}