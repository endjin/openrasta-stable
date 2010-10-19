namespace OpenRasta.Web.UriDecorators
{
    #region Using Directives

    using System;
    using System.Text.RegularExpressions;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Contracts.Web.UriDecorators;
    using OpenRasta.Extensions;

    #endregion

    public class HttpMethodOverrideUriDecorator : IUriDecorator
    {
        private static readonly Regex SegmentRegex = new Regex("!(?<method>[a-zA-Z]+)", RegexOptions.Compiled);
        private readonly ICommunicationContext context;
        private string newVerb;

        public HttpMethodOverrideUriDecorator(ICommunicationContext context)
        {
            this.context = context;
        }

        public bool Parse(Uri uri, out Uri processedUri)
        {
            string[] uriSegments = uri.GetSegments();
            string lastSegment = uriSegments[uriSegments.Length - 1];
            var match = SegmentRegex.Match(lastSegment);
            
            if (match.Success)
            {
                this.newVerb = match.Groups["method"].Value;

                var builder = new UriBuilder(uri)
                    {
                        Path = string.Join(string.Empty, uriSegments, 1, uriSegments.Length - 2) + 
                               SegmentRegex.Replace(lastSegment, string.Empty)
                    };

                processedUri = builder.Uri;

                return true;
            }

            processedUri = uri;
            
            return false;
        }

        public void Apply()
        {
            this.context.Request.HttpMethod = this.newVerb;
        }
    }
}