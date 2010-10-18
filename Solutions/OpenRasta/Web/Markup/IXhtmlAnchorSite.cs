﻿#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Web.Markup
{
    using System;

    using OpenRasta.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Rendering;

    public interface IXhtmlAnchorSite
    {
        IXhtmlAnchor Xhtml { get; }
    }

    public static class IXhtmlAnchorSiteExtensions
    {
        public static IDisposable scope(this IXhtmlAnchorSite site, IContentModel element)
        {
            var nodeWriter = new XhtmlNodeWriter();
            nodeWriter.WriteStartTag(site.Xhtml.AmbientWriter, element);
            return new NodeWriterTerminator(nodeWriter, site.Xhtml.AmbientWriter, element);
        }

        private class NodeWriterTerminator : IDisposable
        {
            private readonly XhtmlNodeWriter nodeWriter;
            private readonly IXhtmlWriter writer;
            private readonly IElement element;

            public NodeWriterTerminator(XhtmlNodeWriter nodeWriter, IXhtmlWriter writer, IElement element)
            {
                this.nodeWriter = nodeWriter;
                this.writer = writer;
                this.element = element;
            }

            public void Dispose()
            {
                this.nodeWriter.WriteEndTag(this.writer, this.element);
            }
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