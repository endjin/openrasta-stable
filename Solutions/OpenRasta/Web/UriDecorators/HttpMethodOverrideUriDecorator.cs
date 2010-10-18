#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Web.UriDecorators
{
    using System;
    using System.Text.RegularExpressions;

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
            this.context.Request.HttpMethod = newVerb;
        }
    }
}

#region Full license
//
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
//
#endregion