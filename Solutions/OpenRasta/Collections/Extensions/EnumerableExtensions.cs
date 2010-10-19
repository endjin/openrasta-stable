namespace OpenRasta.Collections
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class EnumerableExtensions
    {
        public static bool Contains(this IEnumerable<string> source, string value, StringComparison comparisonType)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            foreach (string element in source)
            {
                if (string.Compare(element, value, comparisonType) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (list == null)
            {
                return null;
            }

            foreach (var value in list)
            {
                action(value);
            }

            return list;
        }

        public static TDestination[] ToArray<TSource, TDestination>(this IEnumerable<TSource> source, Func<TSource, TDestination> converter)
        {
            return source.ToList(converter).ToArray();
        }

        public static Dictionary<string, TElement> ToCaseInsensitiveDictionary<TSource, TElement>(
            this IEnumerable<TSource> toConvert,
            Func<TSource, string> keyFinder, 
            Func<TSource, TElement> elementFinder)
        {
            return toConvert.ToDictionary(keyFinder, elementFinder, StringComparer.CurrentCultureIgnoreCase);
        }

        public static TCollectionType ToCollection<TSource, TDestination, TCollectionType>(
            this IEnumerable<TSource> source, Func<TSource, TDestination> converter)
            where TCollectionType : ICollection<TDestination>, new()
        {
            var t = new TCollectionType();
            foreach (var elementToConvert in source)
            {
                try
                {
                    t.Add(converter(elementToConvert));
                }
                catch
                {
                }
            }

            return t;
        }

        public static IList<TDestination> ToList<TSource, TDestination>(this IEnumerable<TSource> source, Func<TSource, TDestination> converter)
        {
            return source.Select(converter).ToList();
        }

        public static bool TryForEach<T>(this IEnumerable<T> collection, Action<T> forAction)
        {
            bool hit = false;
            
            foreach (var item in collection)
            {
                hit = true;
                forAction(item);
            }

            return hit;
        }
    }
}