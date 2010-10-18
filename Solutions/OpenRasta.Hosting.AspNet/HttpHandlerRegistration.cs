namespace OpenRasta.Hosting.AspNet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class HttpHandlerRegistration
    {
        private readonly Regex pathRegex;

        public HttpHandlerRegistration(string verb, string path, string type)
        {
            this.Type = type;
            this.Methods = verb.Split(',').Select(x => x.Trim());
            this.Path = path;
            this.pathRegex = new Regex("^" + Regex.Escape(path).Replace("\\*", ".*") + "/?$");
        }

        public IEnumerable<string> Methods { get; private set; }

        public string Path { get; private set; }

        public string Type { get; private set; }

        public bool Matches(string httpMethod, Uri path)
        {
            if (!this.Methods.Contains("*") && !this.Methods.Any(x => string.CompareOrdinal(x, httpMethod) == 0))
            {
                return false;
            }

            bool simpleMatch = this.pathRegex.IsMatch(path.PathAndQuery);

            if (simpleMatch)
            {
                return true;
            }

            return path.Segments.Any(x => this.pathRegex.IsMatch(x));
        }
    }
}