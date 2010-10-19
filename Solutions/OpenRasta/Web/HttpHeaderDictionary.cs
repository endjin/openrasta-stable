namespace OpenRasta.Web
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;

    #endregion

    /// <summary>
    /// Provides a list of http headers. In dire need of refactoring to use specific header types similar to http digest.
    /// </summary>
    public class HttpHeaderDictionary : IDictionary<string, string>
    {
        private const string HdrContentDisposition = "Content-Disposition";
        private const string HdrContentLength = "Content-Length";
        private const string HdrContentType = "Content-Type";
        private readonly IDictionary<string, string> internalBase = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private ContentDispositionHeader contentDisposition;
        private long? contentLength;
        private MediaType contentType;

        public HttpHeaderDictionary()
        {
        }

        public HttpHeaderDictionary(NameValueCollection sourceDictionary)
        {
            foreach (string key in sourceDictionary.Keys)
            {
                this[key] = sourceDictionary[key];
            }
        }

        public MediaType ContentType
        {
            get { return this.contentType; }
            set { this.SetValue(ref this.contentType, HdrContentType, value); }
        }

        public long? ContentLength
        {
            get { return this.contentLength; }
            set { this.SetValue(ref this.contentLength, HdrContentLength, value); }
        }

        public ContentDispositionHeader ContentDisposition
        {
            get { return this.contentDisposition; }
            set { this.SetValue(ref this.contentDisposition, HdrContentDisposition, value); }
        }

        public int Count
        {
            get { return this.internalBase.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ICollection<string> Keys
        {
            get { return this.internalBase.Keys; }
        }

        public ICollection<string> Values
        {
            get { return this.internalBase.Values; }
        }

        public string this[string key]
        {
            get
            {
                string result;
                if (this.internalBase.TryGetValue(key, out result))
                {
                    return result;
                }

                return null;
            }

            set
            {
                this.internalBase[key] = value;
                this.UpdateValue(key, value);
            }
        }

        public void Add(string key, string value)
        {
            this.internalBase.Add(key, value);
            this.UpdateValue(key, value);
        }

        public bool Remove(string key)
        {
            bool result = this.internalBase.Remove(key);
            this.UpdateValue(key, null);

            return result;
        }

        public bool ContainsKey(string key)
        {
            return this.internalBase.ContainsKey(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return this.internalBase.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            this.internalBase.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.internalBase.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return this.internalBase.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            this.internalBase.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return this.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.internalBase.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void UpdateValue(string headerName, string value)
        {
            if (headerName.Equals(HdrContentType, StringComparison.OrdinalIgnoreCase))
            {
                this.contentType = new MediaType(value);
            }
            else if (headerName.Equals(HdrContentLength, StringComparison.OrdinalIgnoreCase))
            {
                long contentLength;

                if (long.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out contentLength))
                {
                    this.contentLength = contentLength;
                }
            }
            else if (headerName.Equals(HdrContentDisposition, StringComparison.OrdinalIgnoreCase))
            {
                this.contentDisposition = new ContentDispositionHeader(value);
            }
        }

        private void SetValue<T>(ref T typedKey, string key, T value)
        {
            typedKey = value;
            this.internalBase[key] = value == null ? null : value.ToString();
        }
    }
}