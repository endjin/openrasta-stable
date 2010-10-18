#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Web
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;

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

#region Full license
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion