namespace OpenRasta.Testing.Hosting.TestRunner.Scenarios
{
    using System;

    using OpenRasta.IO;
    using OpenRasta.Testing.Hosting.TestRunner.Infrastructure;
    using OpenRasta.Web;

    public class uploading_files : context_uploading_files
    {
        public void can_upload_a_file_as_a_complex_type()
        {
            can_upload_a_file(Uris.FilesComplexType, "application/octet-stream");
        }

        public void can_upload_a_file_as_an_app_octet_stream()
        {
        }

        public void can_upload_a_file_as_an_enumerable_multipart()
        {
            can_upload_a_file(Uris.Files, "application/octet-stream");
        }

        public void can_upload_a_file_as_an_ifile()
        {
            can_upload_a_file(Uris.FilesIfile, "application/octet-stream");
        }
        public void can_upload_file_with_specific_content_type()
        {
            can_upload_a_file(Uris.FilesIfile, MediaType.TextPlain.ToString());
        }
        void can_upload_a_file(string uri, string mediaType)
        {
            given_post_with_file(uri, mediaType);

            when_retrieving_the_response();

            then_response_should_be_200_ok();

            Response.Entity.ContentType.ToString().ShouldBe(mediaType);

            var resultStream = Response.Entity.Stream.ReadToEnd();

            resultStream.ShouldHaveSameElementsAs(_randomBytes);
        }

        void given_post_with_file(string uri, string mediaType)
        {
            given_request_to(uri)
                .Post()
                .Accept(MediaType.ApplicationOctetStream)
                .EntityAsMultipartFormData(
                FormData.File("file", "test.bin", _randomBytes, mediaType),
                FormData.Text("Description", "Do not open this file."));
        }
    }

    public abstract class context_uploading_files : environment_context
    {
        protected static byte[] _randomBytes;

        static context_uploading_files()
        {
            _randomBytes = new byte[10000];
            var random = new Random();
            random.NextBytes(_randomBytes);
        }
    }
}