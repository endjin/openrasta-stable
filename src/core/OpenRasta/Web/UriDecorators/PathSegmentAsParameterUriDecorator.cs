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

    using OpenRasta.Collections;
    using OpenRasta.Handlers;

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
