namespace OpenRasta.Web.UriTemplates
{
    #region Using Directives

    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    #endregion

    public class UriTemplateMatch
    {
        public Uri BaseUri { get; set; }

        public NameValueCollection BoundVariables { get; internal set; }

        public object Data { get; set; }

        public NameValueCollection QueryParameters { get; internal set; }

        public Collection<string> RelativePathSegments { get; internal set; }

        public Uri RequestUri { get; set; }

        public UriTemplate Template { get; set; }

        public Collection<string> WildcardPathSegments { get; internal set; }
    }
}