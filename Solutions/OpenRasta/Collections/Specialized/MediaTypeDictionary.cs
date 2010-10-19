namespace OpenRasta.Collections.Specialized
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Web;

    #endregion

    public class MediaTypeDictionary<TValue> : IEnumerable<TValue>
    {
        private readonly Dictionary<string, IList<TValue>> store = new Dictionary<string, IList<TValue>>();
        private readonly Dictionary<string, List<TValue>> subwildcard = new Dictionary<string, List<TValue>>();
        private readonly List<TValue> wildcard = new List<TValue>();

        public void Add(MediaType mediaType, TValue value)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType", "mediaType is null.");
            }

            if (mediaType.IsWildCard)
            {
                this.wildcard.Add(value);
            }
            else if (mediaType.IsSubtypeWildcard)
            {
                this.GetSubtypeWildcardRegistration(mediaType.TopLevelMediaType).Add(value);
            }
            else
            {
                this.AddIfNotPresent(this.GetForMediaType(mediaType), value);
                this.AddIfNotPresent(this.GetForSubTypeWildcard(mediaType), value);
                this.AddIfNotPresent(this.GetForWildcard(), value);
            }
        }

        public void Clear()
        {
            this.store.Clear();
            this.wildcard.Clear();
            this.subwildcard.Clear();
        }

        public IEnumerable<TValue> Matching(MediaType mediaType)
        {
            // match the cache if a key already exists
            foreach (var item in this.GetForMediaType(mediaType))
            {
                yield return item;
            }

            // try to match subtype
            if (!mediaType.IsTopLevelWildcard && this.subwildcard.ContainsKey(mediaType.TopLevelMediaType))
            {
                foreach (var item in this.subwildcard[mediaType.TopLevelMediaType])
                {
                    yield return item;
                }
            }

            foreach (var item in this.wildcard)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return this.store.SelectMany(key => key.Value).GetEnumerator();
        }

        private void AddIfNotPresent(IList<TValue> list, TValue value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
            }
        }

        private IList<TValue> GetForMediaType(MediaType mediaType)
        {
            return this.GetOrCreate(mediaType.MediaType);
        }

        private IList<TValue> GetForSubTypeWildcard(MediaType mediaType)
        {
            return this.GetOrCreate(mediaType.TopLevelMediaType + "/*");
        }

        private IList<TValue> GetForWildcard()
        {
            return this.GetOrCreate("*/*");
        }

        private IList<TValue> GetOrCreate(string key)
        {
            IList<TValue> value;
            
            if (!this.store.TryGetValue(key, out value))
            {
                this.store[key] = value = new List<TValue>();
            }

            return value;
        }

        private List<TValue> GetSubtypeWildcardRegistration(string topLevelMediaType)
        {
            if (!this.subwildcard.ContainsKey(topLevelMediaType))
            {
                this.subwildcard[topLevelMediaType] = new List<TValue>();
            }

            return this.subwildcard[topLevelMediaType];
        }
    }
}