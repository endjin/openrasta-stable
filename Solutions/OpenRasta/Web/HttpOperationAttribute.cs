namespace OpenRasta.Web
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpOperationAttribute : Attribute
    {
        public HttpOperationAttribute()
        {
            this.ContentType = new MediaType("*/*");
        }

        public HttpOperationAttribute(HttpMethod method) : this()
        {
            this.Method = method.ToString();
        }

        public HttpOperationAttribute(string method) : this()
        {
            this.Method = method;
        }

        public MediaType ContentType { get; set; }

        public string ForUriName { get; set; }

        public string Method { get; set; }

        /// <summary>
        /// Tries to find an HttpOperation attribute on a method. 
        /// </summary>
        /// <param name="mi"></param>
        /// <returns>The instance of the HttpOperation attribute, or null if none were defined.</returns>
        public static HttpOperationAttribute Find(MethodInfo mi)
        {
            try
            {
                return GetCustomAttribute(mi, typeof(HttpOperationAttribute)) as HttpOperationAttribute;
            }
            catch
            {
                return null;
            }
        }

        public bool MatchesHttpMethod(string httpMethod)
        {
            return string.CompareOrdinal(this.Method, httpMethod) == 0;
        }

        public bool MatchesUriName(string uriName)
        {
            return string.Compare(this.ForUriName, uriName, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}