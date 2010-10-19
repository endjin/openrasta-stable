namespace OpenRasta.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text;

    public static class CollectionExtensions
    {
        public static void AddRange<T>(this IList<T> source, IEnumerable<T> newItems)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            foreach (var item in newItems)
            {
                source.Add(item);
            }
        }

        public static NameValueCollection With(this NameValueCollection collection, string name, string value)
        {
            collection.Add(name, value);
            
            return collection;
        }

        public static void RemoveMatching<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
        }

        public static string ToHtmlFormEncoding(this NameValueCollection collection)
        {
            if (collection == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach (var key in collection.Keys)
            {
                sb.Append(key).Append("=").Append(collection[key.ToString()]).Append(";");
            }

            return sb.ToString();
        }

        public static string Find(
            this NameValueCollection collectionToSearch, 
            string name, 
            StringComparison comparisonType)
        {
            for (int i = 0; i < collectionToSearch.Count; i++)
            {
                if (string.Compare(collectionToSearch.GetKey(i), name, comparisonType) == 0)
                {
                    return collectionToSearch[i];
                }
            }

            return null;
        }

        public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> objectToWrap)
        {
            return new ReadOnlyDictionaryWrapper<TKey, TValue>(objectToWrap);
        }

        public static IDictionary<string, string[]> ToDictionary(this NameValueCollection collection)
        {
            var target = new Dictionary<string, string[]>();
            
            foreach (string key in collection.AllKeys)
            {
                target.Add(key, collection.GetValues(key));
            }

            return target;
        }

        public static IEnumerable<TValue> StartingWith<TValue>(
            IDictionary<string, TValue> dic, 
            string prefix, 
            StringComparison comparison)
        {
            foreach (var kv in dic)
            {
                if (kv.Key.StartsWith(prefix, comparison))
                {
                    yield return kv.Value;
                }
            }
        }

        private class ReadOnlyDictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>
        {
            private readonly IDictionary<TKey, TValue> wrapped;

            public ReadOnlyDictionaryWrapper(IDictionary<TKey, TValue> wrappedClass)
            {
                this.wrapped = wrappedClass;
            }

            public int Count
            {
                get { return this.wrapped.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public ICollection<TKey> Keys
            {
                get { return this.wrapped.Keys; }
            }

            public ICollection<TValue> Values
            {
                get { return this.wrapped.Values; }
            }

            public TValue this[TKey key]
            {
                get { return this.wrapped[key]; }
                set { this.ErrorIsReadOnly(); }
            }

            public void Add(KeyValuePair<TKey, TValue> item)
            {
                this.ErrorIsReadOnly();
            }

            public void Clear()
            {
                this.ErrorIsReadOnly();
            }

            public bool Contains(KeyValuePair<TKey, TValue> item)
            {
                return this.wrapped.Contains(item);
            }

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            {
                this.wrapped.CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<TKey, TValue> item)
            {
                return false;
            }

            public void Add(TKey key, TValue value)
            {
                this.ErrorIsReadOnly();
            }

            public bool ContainsKey(TKey key)
            {
                return this.wrapped.ContainsKey(key);
            }

            public bool Remove(TKey key)
            {
                return false;
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                return this.wrapped.TryGetValue(key, out value);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)this.wrapped).GetEnumerator();
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return this.wrapped.GetEnumerator();
            }

            private void ErrorIsReadOnly()
            {
                throw new InvalidOperationException("The dictionary is read-only");
            }
        }
    }
}