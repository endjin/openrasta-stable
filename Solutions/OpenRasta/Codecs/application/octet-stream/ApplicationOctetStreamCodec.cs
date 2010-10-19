namespace OpenRasta.Codecs
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.IO;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Extensions;
    using OpenRasta.TypeSystem;
    using OpenRasta.Web;

    #endregion

    [MediaType("application/octet-stream;q=0.5")]
    [MediaType("*/*;q=0.1")]
    [SupportedType(typeof(IFile))]
    [SupportedType(typeof(Stream))]
    [SupportedType(typeof(byte[]))]
    public class ApplicationOctetStreamCodec : Codec, IMediaTypeReader, IMediaTypeWriter
    {
        public object ReadFrom(IHttpEntity request, IType destinationType, string destinationName)
        {
            if (destinationType.IsAssignableTo<IFile>())
            {
                return new HttpEntityFile(request);
            }

            if (destinationType.IsAssignableTo<Stream>())
            {
                return request.Stream;
            }

            if (destinationType.IsAssignableTo<byte[]>())
            {
                return request.Stream.ReadToEnd();
            }

            return Missing.Value;
        }

        public void WriteTo(object entity, IHttpEntity response, string[] codecParameters)
        {
            if (!this.GetWriters(entity, response).Any(x => x))
            {
                throw new InvalidOperationException();
            }
        }

        private static bool TryProcessAs<T>(object target, Action<T> action) where T : class
        {
            var typedTarget = target as T;
            
            if (typedTarget != null)
            {
                action(typedTarget);
                return true;
            }

            return false;
        }

        private static void WriteFileWithFilename(IFile file, string disposition, IHttpEntity response)
        {
            var contentDispositionHeader = response.Headers.ContentDisposition ?? new ContentDispositionHeader(disposition);

            if (!string.IsNullOrEmpty(file.FileName))
            {
                contentDispositionHeader.FileName = file.FileName;
            }

            if (!string.IsNullOrEmpty(contentDispositionHeader.FileName) ||
                contentDispositionHeader.Disposition != "inline")
            {
                response.Headers.ContentDisposition = contentDispositionHeader;
            }

            if ((file.ContentType != null && file.ContentType != MediaType.ApplicationOctetStream) ||
                (file.ContentType == MediaType.ApplicationOctetStream && response.ContentType == null))
            {
                response.ContentType = file.ContentType;
            }

            using (var stream = file.OpenStream())
            {
                stream.CopyTo(response.Stream);
            }
        }

        private IEnumerable<bool> GetWriters(object entity, IHttpEntity response)
        {
            yield return TryProcessAs<IDownloadableFile>(entity, file => WriteFileWithFilename(file, "attachment", response));
            yield return TryProcessAs<IFile>(entity, file => WriteFileWithFilename(file, "inline", response));
            yield return TryProcessAs<Stream>(entity, stream => stream.CopyTo(response.Stream));
            yield return TryProcessAs<byte[]>(entity, bytes => response.Stream.Write(bytes));
        }
    }
}