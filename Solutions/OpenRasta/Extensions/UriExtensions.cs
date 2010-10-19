namespace OpenRasta.Extensions
{
    using System;

    public static class UriExtensions
    {
        public static string[] GetSegments(this Uri uri)
        {
            return uri.Segments;
        }
 
        public static Uri IgnoreAuthority(this Uri uri)
        {
            var builder = new UriBuilder(uri) { Host = "uritemplate" };
            
            return builder.Uri;
        }
        
        public static Uri IgnorePortAndAuthority(this Uri uri)
        {
            var builder = new UriBuilder(uri) { Host = "uritemplate", Port = 80 };

            return builder.Uri;
        }

        public static Uri IgnoreSchemePortAndAuthority(this Uri uri)
        {
            var builder = new UriBuilder(uri) { Scheme = "http:", Host = "uritemplate", Port = 80 };

            return builder.Uri;
        }

        public static Uri ReplaceAuthority(this Uri uri, Uri baseUri)
        {
            if (baseUri == null)
            {
                return uri;
            }

            var builder = new UriBuilder(uri) { Host = baseUri.Host, Port = baseUri.Port };
            
            return builder.Uri;
        }

        public static Uri EnsureHasTrailingSlash(this Uri uri)
        {
            if (uri.Segments.Length > 1 && !uri.Segments[uri.Segments.Length - 1].EndsWith("/"))
            {
                var builder = new UriBuilder(uri);
                builder.Path += "/";
                uri = builder.Uri;
            }
            
            return uri;
        }

        public static Uri MakeAbsolute(this Uri uri, string baseUri)
        {
            return uri.MakeAbsolute(new Uri(baseUri, UriKind.Absolute));
        }
        
        public static Uri MakeAbsolute(this Uri uri, Uri baseUri)
        {
            return new Uri(baseUri, uri);
        }
        
        public static Uri ForView(this Uri uri, string viewName)
        {
            var builder = new UriBuilder(uri);
            builder.Path += ";" + viewName;
            
            return builder.Uri;
        }
    }
}