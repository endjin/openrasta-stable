namespace OpenRasta.Diagnostics
{
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.TypeSystem.ReflectionBased;

    public static class LogSource<T> where T : ILogSource
    {
        private static string category;

        public static string Category
        {
            get
            {
                if (category == null)
                {
                    var attr = typeof(T).FindAttribute<LogCategoryAttribute>();
                    category = attr != null ? attr.CategoryName : typeof(T).Name;
                }

                return category;
            }
        }
    }
}