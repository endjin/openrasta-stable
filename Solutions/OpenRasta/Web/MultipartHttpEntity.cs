namespace OpenRasta.Web
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;

    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Diagnostics;
    using OpenRasta.Exceptions;

    #endregion

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