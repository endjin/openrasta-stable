namespace OpenRasta.Web.UriDecorators
{
    #region Using Directives

    using System;
    using System.Text.RegularExpressions;

    using OpenRasta.Collections;
    using OpenRasta.Contracts.Handlers;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Contracts.Web.UriDecorators;

    #endregion

    public class PathSegmentAsParameterUriDecorator : IUriDecorator
    {
        private static readonly Regex SegmentRegex = new Regex(";(?<segment>[a-zA-Z0-9-=]+)", RegexOptions.Compiled);
        private readonly ICommunicationContext context;

        private IHandlerRepository handlers;
        private string[] matchingSegments = null;

        public PathSegmentAsParameterUriDecorator(ICommunicationContext context, IHandlerRepository handlers)
        {
            this.context = context;
            this.handlers = handlers;
        }
        
        public bool Parse(Uri uri, out Uri processedUri)
        {
            string[] uriSegments = uri.Segments;
            string lastSegment = uriSegments[uriSegments.Length - 1];
            var matches = SegmentRegex.Matches(lastSegment);
            
            if (matches.Count > 0)
            {
                this.matchingSegments = new string[matches.Count];
                
                for (int i = 0; i < matches.Count; i++)
                {
                    this.matchingSegments[i] = matches[i].Groups["segment"].Value;
                }
                
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
            this.context.Request.CodecParameters.AddRange(this.matchingSegments);
        }
    }
}