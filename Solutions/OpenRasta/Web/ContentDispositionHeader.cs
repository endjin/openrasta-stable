#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Web
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using OpenRasta.Text;

    public class ContentDispositionHeader : IEquatable<ContentDispositionHeader>
    {
        public ContentDispositionHeader(string header)
        {
            var fragments = header.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (fragments.Length == 0)
            {
                throw new FormatException("The header value {0} is invalid for Content-Disposition.".With(header));
            }

            this.Disposition = fragments[0].Trim();

            for (int i = 1; i < fragments.Length; i++)
            {
                var parameter = this.ParseParameter(fragments[i]);
                
                if (string.Compare(parameter.Key, "filename", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.FileName = parameter.Value;
                }
                else if (string.Compare(parameter.Key, "name", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.Name = Rfc2047Encoding.DecodeTextToken(parameter.Value);
                }
            }
        }

        public string FileName { get; set; }

        public string Name { get; set; }

        public string Disposition { get; set; }

        public override string ToString()
        {
            var header = new StringBuilder();
            header.Append(this.Disposition);
            
            if (this.Name != null)
            {
                header.Append("; name=\"").Append(this.Name).Append("\"");
            }
            
            if (this.FileName != null)
            {
                header.Append("; filename=\"").Append(this.FileName).Append("\"");
            }

            return header.ToString();
        }

        public bool Equals(ContentDispositionHeader other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Disposition == other.Disposition && this.Name == other.Name && this.FileName == other.FileName;
        }

        public override int GetHashCode()
        {
            return (this.Disposition ?? string.Empty).GetHashCode() ^ (this.Name ?? string.Empty).GetHashCode() ^ (this.FileName ?? string.Empty).GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is ContentDispositionHeader) || obj == null)
            {
                return false;
            }

            return this.Equals((ContentDispositionHeader)obj);
        }

        private KeyValuePair<string, string> ParseParameter(string fragment)
        {
            var equalIndex = fragment.IndexOf('=');
            
            if (equalIndex == -1)
            {
                throw new FormatException();
            }
            
            var key = fragment.Substring(0, equalIndex).Trim();
            var beginningValue = fragment.IndexOf('"', equalIndex + 1);
            
            if (beginningValue == -1)
            {
                throw new FormatException();
            }

            var endValue = fragment.IndexOf('"', beginningValue + 1);

            if (endValue == -1)
            {
                throw new FormatException();
            }

            return new KeyValuePair<string, string>(key, fragment.Substring(beginningValue + 1, endValue - beginningValue - 1));
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
