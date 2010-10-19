namespace OpenRasta.Web
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;

    #endregion

    public class UriRegistration
    {
        public UriRegistration(string uriTemplate, object resourceKey) : this(uriTemplate, resourceKey, null, null)
        {
        }

        public UriRegistration(string uriTemplate, object resourceKey, string uriName, CultureInfo uriCulture)
        {
            this.UriTemplate = uriTemplate;
            this.UriTemplateParameters = new List<NameValueCollection>();
            this.ResourceKey = resourceKey;
            this.UriName = uriName;
            this.UriCulture = uriCulture;
        }

        public IList<NameValueCollection> UriTemplateParameters { get; private set; }

        public object ResourceKey { get; private set; }

        public string UriName { get; private set; }

        public CultureInfo UriCulture { get; private set; }

        public string UriTemplate { get; private set; }
    }
}