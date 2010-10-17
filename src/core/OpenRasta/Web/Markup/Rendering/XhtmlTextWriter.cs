#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Web.Markup.Rendering
{
    using System;
    using System.IO;
    using System.Text;

    public class XhtmlTextWriter : IXhtmlWriter, ISupportsTextWriter
    {
        private const string TagAttr = " {0}=\"{1}\"";
        private const string TagEnd = "</{0}>";
        private const string TagStartBegin = "<{0}";
        private const string TagStartEnd = ">";
        private const string TagStartEndFinal = " />";

        public XhtmlTextWriter(TextWriter source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "source is null.");
            }

            TextWriter = source;
        }

        public TextWriter TextWriter
        {
            get; private set;
        }

        public static string HtmlEncode(string source)
        {
            if (source == null)
            {
                return null;
            }

            var builder = new StringBuilder();
            
            foreach (var c in source)
            {
                switch (c)
                {
                    case '"':
                    case '\'':
                    case '&':
                    case '<':
                    case '>':
                        builder.Append("&#").Append((int)c).Append(';');
                        break;
                    default:
                        builder.Append(c);
                        break;
                }
            }

            return builder.ToString();
        }

        public void BeginWriteStartElement(string tagName)
        {
            TextWriter.Write(TagStartBegin.With(tagName));
        }

        public void EndWriteStartElement()
        {
            TextWriter.Write(TagStartEnd);
        }

        public void EndWriteStartElementFinal()
        {
            TextWriter.Write(TagStartEndFinal);
        }

        public void WriteEndElement(string tagName)
        {
            TextWriter.Write(TagEnd.With(tagName));
        }

        public void WriteAttributeString(string key, string value)
        {
            TextWriter.Write(TagAttr.With(key, HtmlEncode(value)));
        }

        public void WriteString(string content)
        {
            TextWriter.Write(HtmlEncode(content));
        }

        public void WriteUnencodedString(string content)
        {
            TextWriter.Write(content);
        }
    }
}

#region Full license
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
#endregion