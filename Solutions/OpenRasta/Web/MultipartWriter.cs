namespace OpenRasta.Web
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Extensions;

    #endregion

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