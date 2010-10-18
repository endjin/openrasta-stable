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

    using OpenRasta.Codecs;
    using OpenRasta.Diagnostics;

    public interface IMultipartHttpEntity : IHttpEntity
    {
        void SwapStream(string filepath);

        void SwapStream(Stream stream);
    }

    public class MultipartHttpEntity : IMultipartHttpEntity, IDisposable
    {
        private bool disposed;
        private string filePath;
        private Stream internalStream;

        public MultipartHttpEntity()
        {
            this.Headers = new HttpHeaderDictionary();
        }

        ~MultipartHttpEntity()
        {
            this.Dispose(false);
        }

        public ICodec Codec { get; set; }

        public MediaType ContentType
        {
            get { return this.Headers.ContentType; }
            set { this.Headers.ContentType = value; }
        }

        public long? ContentLength
        {
            get { return this.Headers.ContentLength; }
            set { this.Headers.ContentLength = value; }
        }

        public Stream Stream
        {
            get
            {
                if (this.internalStream == null && File.Exists(this.filePath))
                {
                    this.internalStream = File.Open(this.filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                }

                return this.internalStream;
            }

            set
            {
                this.internalStream = value;
                this.filePath = null;
            }
        }

        public IList<Error> Errors { get; private set; }

        public HttpHeaderDictionary Headers { get; private set; }

        public object Instance { get; set; }

        private ILogger Log { get; set; }

        public void SwapStream(Stream stream)
        {
            Stream = stream;
        }

        public void SwapStream(string filepath)
        {
            this.filePath = filepath;
            this.internalStream = null;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.internalStream != null)
                    {
                        try
                        {
                            this.internalStream.Dispose();
                        }
                        catch (ObjectDisposedException)
                        {
                        }
                        finally
                        {
                            this.internalStream = null;
                        }
                    }

                    if (this.filePath != null && File.Exists(this.filePath))
                    {
                        try
                        {
                            File.Delete(this.filePath);
                        }
                        catch (Exception e)
                        {
                            this.Log.Safe().WriteError("Could not delete file {0} after use. See exception for details.", this.filePath);
                            this.Log.Safe().WriteException(e);
                        }
                        finally
                        {
                            this.filePath = null;
                        }
                    }
                }

                this.disposed = true;
            }
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
