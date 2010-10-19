namespace OpenRasta.Tests.Unit.Codecs.MultipartFormDataCodec
{
    #region Using Directives

    using System.Collections.Generic;

    using NUnit.Framework;

    using OpenRasta.Testing.Framework.Fakes;
    using OpenRasta.Testing.Specifications;
    using OpenRasta.Tests.Unit.Codecs.MediaTypeDictionary;

    #endregion

    public class when_parsing_parts_with_names_representing_types : multipart_codec
    {
        [Test]
        public void a_string_property_is_assigned()
        {
            given_context();
            
            GivenAMultipartRequestStream(Scenarios.TwoFieldsComposedNames);

            when_decoding<Customer>("customer");

            then_decoding_result<Customer>().Username.ShouldBe("johndoe");
        }

        [Test]
        public void a_datetime_property_is_assigned()
        {
            given_context();
            
            GivenAMultipartRequestStream(Scenarios.TwoFieldsComposedNames);

            when_decoding<Customer>("customer");

            then_decoding_result<Customer>().DateOfBirth.Year.ShouldBe(2001);
        }

        [Test]
        public void another_mime_type_for_key_values_is_used_to_parse_the_result_correctly()
        {
            given_context();
            
            GivenAMultipartRequestStream(Scenarios.TwoFieldsComposedNames);

            when_decoding<IDictionary<string, string[]>>("additions");

            then_decoding_result<IDictionary<string, string[]>>()["oneplusone"][0].ShouldBe("two");
            then_decoding_result<IDictionary<string, string[]>>()["oneplustwo"][0].ShouldBe("three");
        }

        [Test]
        public void construction_of_objects_from_other_media_types_returns_the_correct_values()
        {
            given_context();
            
            GivenAMultipartRequestStream(Scenarios.NestedContentTypes);

            when_decoding<Customer>("customer");

            then_decoding_result<Customer>().DateOfBirth.Year.ShouldBe(2001);
            then_decoding_result<Customer>().DateOfBirth.Month.ShouldBe(12);
            then_decoding_result<Customer>().DateOfBirth.Day.ShouldBe(10);            
        }
    }
}