namespace OpenRasta.Tests.Unit.Codecs.ApplicationOctetStreamCodec
{
    #region Using Directives

    using System.IO;

    using NUnit.Framework;

    using OpenRasta.Testing.Specifications;

    #endregion

    [TestFixture]
    public class when_converting_a_byte_stream_to_an_instance_of_a_stream : applicationoctetstream_context
    {
        [Test]
        public void the_stream_length_is_set_to_the_size_of_the_sent_byte_stream()
        {
            given_context();
            given_request_entity_stream();
            given_content_disposition_header("attachment;filename=\"test.txt\"");

            WhenParsing();

            ThenTheResult.Length.ShouldBe(1024);
            
        }

        public void WhenParsing()
        {
            when_decoding<Stream>();
        }

        public Stream ThenTheResult { get
        {
            return then_decoding_result<Stream>();
        } }
    }
}