namespace OpenRasta.Collections.Specialized
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Reflection;

    public static class SpecializedCollectionExtensions
    {
        public static void AddOrReplace<TKey, TValue>(
            this IDictionary<TKey, TValue> target, 
            IDictionary<TKey, TValue> values)
        {
            foreach (var key in values.Keys)
            {
                if (target.ContainsKey(key))
                {
                    target[key] = values[key];
                }
                else
                {
                    target.Add(key, values[key]);
                }
            }
        }

        public static void AddReplace(this NameValueCollection baseCollection, NameValueCollection collection)
        {
            foreach (string key in collection.AllKeys)
            {
                baseCollection[key] = collection[key];
            }
        }

        public static IDictionary<string, string> ToCaseInvariantDictionary(this object objectToConvert)
        {
            return ToDictionary(objectToConvert, StringComparer.OrdinalIgnoreCase);
        }

        public static IDictionary<string, string> ToDictionary(this object objectToConvert)
        {
            return ToDictionary(objectToConvert, StringComparer.CurrentCulture);
        }

        public static IDictionary<string, string> ToDictionary(this object objectToConvert, IEqualityComparer<string> comparer)
        {
            if (objectToConvert is IDictionary<string, string>)
            {
                return objectToConvert as IDictionary<string, string>;
            }

            var dic = new Dictionary<string, string>(comparer);
                
            foreach (var value in GetValues(objectToConvert))
            {
                dic.Add(value.Key, value.Value == null ? null : value.Value.ToString());
            }

            return dic;
        }

        public static NameValueCollection ToNameValueCollection(this object objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }

            if (objectToConvert is NameValueCollection)
            {
                return (NameValueCollection)objectToConvert;
            }

            var values = new NameValueCollection();

            foreach (var value in GetValues(objectToConvert))
            {
                values.Add(value.Key, value.Value != null ? value.Value.ToString() : null);
            }

            return values;
        }

        public static IDictionary<string, object> ToProperties(this object objectToConvert)
        {
            if (objectToConvert is IDictionary<string, object>)
            {
                return objectToConvert as IDictionary<string, object>;
            }
                
            var dic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                
            foreach (var value in GetValues(objectToConvert))
            {
                dic.Add(value.Key, value.Value);
            }

            return dic;
        }

        private static IEnumerable<KeyValuePair<string, object>> GetValues(object obj)
        {
            var objType = obj.GetType();

            foreach (var pi in objType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (pi.GetIndexParameters().Length == 0)
                {
                    yield return new KeyValuePair<string, object>(pi.Name, pi.GetValue(obj, null));
                }
            }
        }
    }
}