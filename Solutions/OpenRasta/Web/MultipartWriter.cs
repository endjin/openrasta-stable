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
    using System.IO;
    using System.Text;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Extensions;
    using OpenRasta.IO;

    public class MultipartWriter : IDisposable
    {
        private readonly Stream underlyingStream;
        private readonly byte[] beginBoundary;
        private readonly byte[] endBoundary;
        private readonly Encoding encoding;

        private string boundary;

        public MultipartWriter(string boundary, Stream underlyingStream, Encoding encoding)
        {
            this.boundary = boundary;
            this.underlyingStream = underlyingStream;
            this.encoding = encoding;
            this.beginBoundary = encoding.GetBytes("--" + boundary + "\r\n");
            this.endBoundary = encoding.GetBytes("\r\n--" + boundary + "--\r\n");
        }

        public void Close()
        {
            this.underlyingStream.Write(this.endBoundary);
        }

        public void Dispose()
        {
            this.Close();
        }

        public void Write(IHttpEntity formDataField)
        {
            this.WriteLine();
            this.WriteBoundary();
            
            foreach (var header in formDataField.Headers)
            {
                this.WriteHeader(header);
            }
            
            this.WriteContentLength(formDataField);
            this.WriteLine();
            this.WriteBody(formDataField);
        }

        private void WriteBoundary()
        {
            this.underlyingStream.Write(this.beginBoundary, 0, this.beginBoundary.Length);
        }

        private void WriteContentLength(IHttpEntity formDataField)
        {
            if (formDataField.ContentLength != null)
            {
                if (formDataField.Stream != null && formDataField.Stream.CanSeek)
                {
                    this.WriteHeader(new KeyValuePair<string, string>("Content-Length", formDataField.Stream.Length.ToString()));
                }
                else if (formDataField.Stream == null)
                {
                    this.WriteHeader(new KeyValuePair<string, string>("Content-Length", "0"));
                }
            }
        }

        private void WriteBody(IHttpEntity formDataField)
        {
            formDataField.Stream.CopyTo(this.underlyingStream);
        }

        private void WriteLine()
        {
            this.underlyingStream.Write(new byte[] { 13, 10 }, 0, 2);
        }

        private void WriteHeader(KeyValuePair<string, string> header)
        {
            // TODO: handle split header scenarios and non-ascii chars
            string serializedHeader = header.Key + ": " + header.Value + "\r\n";
            byte[] resultHeader = this.encoding.GetBytes(serializedHeader);
            this.underlyingStream.Write(resultHeader);
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