namespace OpenRasta.Testing.Hosting.Handlers
{
    using System.Collections.Generic;
    using System.IO;

    using OpenRasta.Contracts.IO;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Extensions;
    using OpenRasta.IO;
    using OpenRasta.Testing.Hosting.Resources;
    using OpenRasta.Web;

    public class UploadedFileHandler
    {
        private static readonly List<IFile> Files = new List<IFile>();

        public OperationResult Get(int id)
        {
            if (Files.Count < id)
            {
                return new OperationResult.NotFound();
            }

            var streamToSend = Files[id];
            streamToSend.OpenStream().Position = 0;
            
            return new OperationResult.OK(streamToSend);
        }

        /// <summary>
        /// Used to test that files sent as multipart/form-data in html forms get processed correctly
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public OperationResult Post(IEnumerable<IMultipartHttpEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (!entity.Headers.ContentDisposition.Disposition.EqualsOrdinalIgnoreCase("form-data"))
                {
                    return new OperationResult.BadRequest { ResponseResource = "Sent a field that is not declared as form-data, cannot process" };
                }

                return new OperationResult.SeeOther
                    {
                        RedirectLocation = typeof(UploadedFile).CreateUri(new { id = this.ReceiveStream(entity.ContentType, entity.Stream) })
                    };
            }

            return new OperationResult.BadRequest { ResponseResource = "Sent multiple files, cannot process the request" };
        }

        [HttpOperation(ForUriName = "IFile")]
        public OperationResult Post(IFile file)
        {
            return new OperationResult.SeeOther
                {
                    RedirectLocation = typeof(UploadedFile).CreateUri(new { id = this.ReceiveStream(file.ContentType, file.OpenStream()) })
                };
        }

        [HttpOperation(ForUriName = "complexType")]
        public OperationResult Post(UploadedFile uploadedFile)
        {
            return new OperationResult.SeeOther
                {
                    RedirectLocation = typeof(UploadedFile).CreateUri(new { id = this.ReceiveStream(uploadedFile.File.ContentType, uploadedFile.File.OpenStream()) })
                };
        }

        /// <summary>
        /// Used to test simple put requests using application/octet-stream
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public OperationResult Post(string fileName, Stream fileStream)
        {
            return new OperationResult.SeeOther
            {
                RedirectLocation = typeof(UploadedFile).CreateUri(new { id = this.ReceiveStream(MediaType.ApplicationOctetStream, fileStream) })
            };
        }

        private int ReceiveStream(MediaType streamType, Stream stream)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);

            var file = new InMemoryFile(memoryStream) { ContentType = streamType };
            Files.Add(file);

            return Files.IndexOf(file);
        }
    }
}