namespace OpenRasta.Extensions
{
    using System;
    using System.Text;

    public static class StringExtensions
    {
        public static bool EqualsOrdinalIgnoreCase(this string target, string compare)
        {
            return string.Compare(target, compare, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool IsEmpty(this string target)
        {
            return target == string.Empty;
        }

        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        public static bool IsNullOrWhiteSpace(this string target)
        {
            return string.IsNullOrEmpty(target) || target.IsWhiteSpace();
        }

        public static bool IsWhiteSpace(this string target)
        {
            return target.Trim() == string.Empty;
        }

        public static string FromBase64String(this string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        public static Uri ToUri(this string target)
        {
            return ToUri(target, UriKind.RelativeOrAbsolute);
        }

        public static Uri ToUri(this string target, UriKind uriKind)
        {
            if (target == null)
            {
                return null;
            }

            return new Uri(target, uriKind);
        }

        public static string With(this string target, params object[] parameters)
        {
            if (target == null)
            {
                return null;
            }

            if (parameters == null || parameters.Length == 0)
            {
                return target;
            }

            return string.Format(target, parameters);
        }
    }
}