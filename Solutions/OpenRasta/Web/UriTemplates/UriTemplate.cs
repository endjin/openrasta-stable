namespace OpenRasta.Web.UriTemplates
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;

    #endregion

    public class UriTemplate
    {
        private const string WildcardText = "*";
        private readonly Dictionary<string, UrlSegment> pathSegmentVariables;
        private readonly List<UrlSegment> internalSegments;
        private readonly Dictionary<string, QuerySegment> templateQuerySegments;
        private readonly Uri internalTemplateUri;

        public UriTemplate(string template)
        {
            this.internalTemplateUri = ParseTemplate(template);
            this.internalSegments = ParseSegments(this.internalTemplateUri);
            this.pathSegmentVariables = ParseSegmentVariables(this.internalSegments);
            this.templateQuerySegments = ParseQueries(this.internalTemplateUri);

            this.PathSegmentVariableNames = new ReadOnlyCollection<string>(new List<string>(this.pathSegmentVariables.Keys));
            this.QueryValueVariableNames = new ReadOnlyCollection<string>(new List<string>(this.GetQueryValueVariableNames(this.templateQuerySegments)));
        }

        private enum SegmentType
        {
            Wildcard,
            Variable,
            Literal
        }

        public ReadOnlyCollection<string> PathSegmentVariableNames { get; private set; }

        public ReadOnlyCollection<string> QueryValueVariableNames { get; private set; }

        public Uri BindByName(Uri baseAddress, NameValueCollection parameters)
        {
            if (baseAddress == null)
            {
                throw new ArgumentNullException("baseAddress", "The base Uri needs to be provided for a Uri to be generated.");
            }

            baseAddress = SanitizeUriAsBaseUri(baseAddress);

            var path = new StringBuilder();
            
            foreach (UrlSegment segment in this.internalSegments)
            {
                if (segment.Type == SegmentType.Literal)
                {
                    path.Append(segment.Text);
                }
                else if (segment.Type == SegmentType.Variable)
                {
                    path.Append(parameters[segment.Text.ToUpperInvariant()]);
                }

                if (segment.TrailingSeparator)
                {
                    path.Append('/');
                }
            }

            if (this.templateQuerySegments.Count > 0)
            {
                path.Append('?');
                
                foreach (var querySegment in this.templateQuerySegments)
                {
                    path.Append(querySegment.Value.Key).Append("=").Append(parameters[querySegment.Value.Value]).Append(";");
                }
                
                path.Remove(path.Length - 1, 1);
            }
            
            return new Uri(baseAddress, path.ToString());
        }

        public Uri BindByPosition(Uri baseAddress, params string[] values)
        {
            baseAddress = SanitizeUriAsBaseUri(baseAddress);
            var path = new StringBuilder();
            int paramPosition = 0;
            
            foreach (UrlSegment segment in this.internalSegments)
            {
                if (segment.Type == SegmentType.Literal)
                {
                    path.Append(segment.Text);
                }
                else if (segment.Type == SegmentType.Variable)
                {
                    string param = paramPosition < values.Length ? values[paramPosition++] : segment.Text;
                    path.Append(param);
                }

                if (segment.TrailingSeparator)
                {
                    path.Append('/');
                }
            }

            return new Uri(baseAddress, path.ToString());
        }

        public bool IsEquivalentTo(UriTemplate other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.internalSegments.Count != other.internalSegments.Count)
            {
                return false;
            }
            
            if (this.templateQuerySegments.Count != other.templateQuerySegments.Count)
            {
                return false;
            }
            
            for (int i = 0; i < this.internalSegments.Count; i++)
            {
                UrlSegment thisSegment = this.internalSegments[i];
                UrlSegment otherSegment = other.internalSegments[i];
                
                if (thisSegment.Type != otherSegment.Type)
                {
                    return false;
                }
                
                if (thisSegment.Type == SegmentType.Literal && thisSegment.Text != otherSegment.Text)
                {
                    return false;
                }
            }

            foreach (var thisSegment in this.templateQuerySegments)
            {
                if (!other.templateQuerySegments.ContainsKey(thisSegment.Key))
                {
                    return false;
                }

                QuerySegment otherSegment = other.templateQuerySegments[thisSegment.Key];

                if (thisSegment.Value.Type != otherSegment.Type)
                {
                    return false;
                }

                if (thisSegment.Value.Type == SegmentType.Literal && thisSegment.Value.Value != otherSegment.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public UriTemplateMatch Match(Uri baseAddress, Uri candidate)
        {
            if (baseAddress == null || candidate == null)
            {
                return null;
            }

            if (baseAddress.GetLeftPart(UriPartial.Authority) != candidate.GetLeftPart(UriPartial.Authority))
            {
                return null;
            }

            var baseUriSegments = baseAddress.Segments.Select(s => this.RemoveTrailingSlash(s));
            var candidateSegments = new List<string>(candidate.Segments.Select(x => this.RemoveTrailingSlash(x)));

            foreach (var baseUriSegment in baseUriSegments)
            {
                if (baseUriSegment == candidateSegments[0])
                {
                    candidateSegments.RemoveAt(0);
                }
            }

            if (candidateSegments.Count > 0 && candidateSegments[0] == string.Empty)
            {
                candidateSegments.RemoveAt(0);
            }

            if (candidateSegments.Count != this.internalSegments.Count)
            {
                return null;
            }

            var boundVariables = new NameValueCollection(this.pathSegmentVariables.Count);
            
            for (int i = 0; i < this.internalSegments.Count; i++)
            {
                string segment = candidateSegments[i];

                var candidateSegment = new { Text = segment, ProposedSegment = this.internalSegments[i] };

                candidateSegments[i] = candidateSegment.Text;

                if (candidateSegment.ProposedSegment.Type == SegmentType.Literal
                    && string.Compare(candidateSegment.ProposedSegment.Text, segment, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return null;
                }

                if (candidateSegment.ProposedSegment.Type == SegmentType.Wildcard)
                {
                    throw new NotImplementedException("Not finished wildcards implementation yet");
                }

                if (candidateSegment.ProposedSegment.Type == SegmentType.Variable)
                {
                    boundVariables.Add(candidateSegment.ProposedSegment.Text, Uri.UnescapeDataString(candidateSegment.Text));
                }
            }

            var queryMatches = new NameValueCollection();
            Dictionary<string, QuerySegment> requestQueryString = ParseQueries(candidate);

            foreach (QuerySegment querySegment in this.templateQuerySegments.Values)
            {
                // if you match text exactly
                if (querySegment.Type == SegmentType.Literal && (!requestQueryString.ContainsKey(querySegment.Key)
                                                                 || requestQueryString[querySegment.Key].Value != querySegment.Value))
                {
                    return null;
                }
                
                if (querySegment.Type == SegmentType.Variable)
                {
                    if (!requestQueryString.ContainsKey(querySegment.Key))
                    {
                        return null;
                    }

                    // TODO: Decide on a behavior when there is several queries of the same name. Think it should turn it into an array
                    queryMatches[querySegment.Value] = requestQueryString[querySegment.Key].Value;
                }
            }

            return new UriTemplateMatch
            {
                BaseUri = baseAddress,
                Data = 0,
                BoundVariables = boundVariables,
                QueryParameters = queryMatches,
                RelativePathSegments = new Collection<string>(candidateSegments),
                RequestUri = candidate,
                Template = this,
                WildcardPathSegments = new Collection<string>()
            };
        }

        public override int GetHashCode()
        {
            int hash = 0;

            foreach (UrlSegment segment in this.internalSegments)
            {
                hash ^= segment.OriginalText.GetHashCode();
            }

            return hash;
        }

        public override string ToString()
        {
            return Uri.UnescapeDataString(this.internalTemplateUri.AbsolutePath);
        }

        private static Dictionary<string, QuerySegment> ParseQueries(Uri templateUri)
        {
            string queries = templateUri.Query;
            string[] pairs = queries.Split('&');
            var nc = new Dictionary<string, QuerySegment>();
            
            foreach (string value in pairs)
            {
                string unescapedString = Uri.UnescapeDataString(value);
                
                if (unescapedString.Length == 0)
                {
                    continue;
                }
                
                int variableStart = unescapedString[0] == '?' ? 1 : 0;

                int equalSignPosition = unescapedString.IndexOf('=');
                
                if (equalSignPosition != -1)
                {
                    string val = unescapedString.Substring(equalSignPosition + 1);
                    string valAsVariable = GetVariableName(val);
                    
                    var segment = new QuerySegment
                        {
                            Key = unescapedString.Substring(variableStart, equalSignPosition - variableStart),
                            Value = valAsVariable ?? val,
                            Type = valAsVariable == null ? SegmentType.Literal : SegmentType.Variable
                        };
                    
                    nc[segment.Key] = segment;
                }
            }

            return nc;
        }

        private static List<UrlSegment> ParseSegments(Uri templateUri)
        {
            var pasedSegments = new List<UrlSegment>();
            string[] originalSegments = templateUri.Segments;
     
            foreach (string segmentText in originalSegments)
            {
                UrlSegment parsedSegment;
                string unescapedSegment = Uri.UnescapeDataString(segmentText);
                string sanitizedSegment = unescapedSegment.Replace("/", string.Empty);
                bool trailingSeparator = unescapedSegment.Length - sanitizedSegment.Length > 0;
                string variableName;

                // this is the '/' returned by Uri which we don't care much for
                if (sanitizedSegment == string.Empty) 
                {
                    continue;
                }

                if ((variableName = GetVariableName(unescapedSegment)) != null)
                {
                    parsedSegment = new UrlSegment
                        {
                            Text = variableName,
                            OriginalText = sanitizedSegment,
                            Type = SegmentType.Variable,
                            TrailingSeparator = trailingSeparator
                        };
                }
                else if (string.Compare(unescapedSegment, WildcardText, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    parsedSegment = new UrlSegment
                        {
                            Text = WildcardText, OriginalText = sanitizedSegment, Type = SegmentType.Wildcard
                        };
                }
                else
                {
                    parsedSegment = new UrlSegment
                        {
                            Text = sanitizedSegment,
                            OriginalText = sanitizedSegment,
                            Type = SegmentType.Literal,
                            TrailingSeparator = trailingSeparator
                        };
                }

                pasedSegments.Add(parsedSegment);
            }

            return pasedSegments;
        }

        private static Dictionary<string, UrlSegment> ParseSegmentVariables(List<UrlSegment> segments)
        {
            var returnDic = new Dictionary<string, UrlSegment>();

            foreach (UrlSegment segment in segments)
            {
                if (segment.Type == SegmentType.Variable)
                {
                    returnDic.Add(segment.Text.ToUpperInvariant(), segment);
                }
            }

            return returnDic;
        }

        private static Uri ParseTemplate(string template)
        {
            return new Uri(new Uri("http://uritemplateimpl"), template);
        }

        private static Uri SanitizeUriAsBaseUri(Uri address)
        {
            var builder = new UriBuilder(address) { Fragment = null, Query = null };

            if (!builder.Path.EndsWith("/"))
            {
                builder.Path += "/";
            }

            return builder.Uri;
        }

        private static string GetVariableName(string segmentText)
        {
            segmentText = segmentText.Replace("/", string.Empty).Trim();

            string result = null;

            if (segmentText.Length > 2 && segmentText[0] == '{' && segmentText[segmentText.Length - 1] == '}')
            {
                result = segmentText.Substring(1, segmentText.Length - 2);
            }

            return result;
        }

        private IEnumerable<string> GetQueryValueVariableNames(Dictionary<string, QuerySegment> valueCollection)
        {
            foreach (var qsegment in valueCollection)
            {
                if (qsegment.Value.Type == SegmentType.Variable)
                {
                    yield return qsegment.Value.Value;
                }
            }
        }

        private string RemoveTrailingSlash(string str)
        {
            return str.LastIndexOf('/') == str.Length - 1 ? str.Substring(0, str.Length - 1) : str;
        }

        private class QuerySegment
        {
            public string Key { get; set; }

            public string Value { get; set; }

            public SegmentType Type { get; set; }
        }

        private class UrlSegment
        {
            public string OriginalText { get; set; }

            public string Text { get; set; }

            public bool TrailingSeparator { get; set; }

            public SegmentType Type { get; set; }
        }
    }
}