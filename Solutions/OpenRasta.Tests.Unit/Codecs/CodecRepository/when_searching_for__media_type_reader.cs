#region License

//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

#endregion

namespace OpenRasta.Tests.Unit.Codecs.CodecRepository
{
    #region Using Directives

    using NUnit.Framework;

    using OpenRasta.Codecs;
    using OpenRasta.IO;
    using OpenRasta.Testing.Framework.Fakes;
    using OpenRasta.Testing.Specifications;

    #endregion

    public class when_searching_for__media_type_reader : codec_repository_context
    {
        [Test]
        public void a_codec_for_a_parent_resource_type_is_found()
        {
            GivenACodec<CustomerCodec, object>("application/xml");

            WhenFindingCodec("application/xml", typeof(Customer));

            this.ThenTheResult.CodecType
                .ShouldBe<CustomerCodec>();
        }

        [Test]
        public void a_codec_for_a_parent_resource_type_with_strict_marker_is_not_found()
        {
            GivenACodec<CustomerCodec, Strictly<object>>("application/xml");

            WhenFindingCodec("application/xml", typeof(Customer));

            this.ThenTheResult.ShouldBeNull();
        }

        [Test]
        public void a_codec_for_the_exact_resource_type_is_found()
        {
            GivenACodec<CustomerCodec, Strictly<Customer>>("application/xml");

            WhenFindingCodec("application/xml", typeof(Customer));

            this.ThenTheResult.ShouldNotBeNull().CodecType.ShouldBe<CustomerCodec>();
        }

        [Test]
        public void a_codec_that_is_not_registered_for_all_resource_types_is_not_selected()
        {
            GivenACodec<CustomerCodec, Customer>("application/xml");

            WhenFindingCodec("application/xml", typeof(Customer), typeof(Frodo));

            this.ThenTheResult.ShouldBeNull();
        }

        [Test]
        public void a_wildcard_codec_is_not_selected_wheN_another_codec_has_matching_media_type()
        {
            GivenACodec<ApplicationOctetStreamCodec, IFile>("*/*;q=0.5");
            GivenACodec<MultipartFormDataObjectCodec, IFile>("multipart/form-data;q=0.5");

            WhenFindingCodec("multipart/form-data", typeof(IFile));

            this.ThenTheResult.CodecType.ShouldBe<MultipartFormDataObjectCodec>();
        }

        [Test]
        public void a_wildcard_codec_is_selected_if_the_destination_type_is_closest_to_the_param_type()
        {
            GivenACodec<ApplicationOctetStreamCodec, IFile>("*/*;q=0.4");
            GivenACodec<TextPlainCodec, string>("text/plain;q=0.5");

            WhenFindingCodec("text/plain", typeof(IFile));

            this.ThenTheResult.CodecType.ShouldBe<ApplicationOctetStreamCodec>();
        }

        [Test]
        public void a_wildcard_selects_the_codec_with_the_highest_quality()
        {
            GivenACodec<CustomerCodec, object>("application/json;q=0.4");
            GivenACodec<AnotherCustomerCodec, object>("application/xml;q=0.3");

            WhenFindingCodec("*/*", typeof(Customer));

            this.ThenTheResult.CodecType.ShouldBe<CustomerCodec>();
        }

        [Test]
        public void the_codec_with_the_highest_quality_is_selected()
        {
            GivenACodec<CustomerCodec, object>("application/xml;q=0.9", "nonspecific");
            GivenACodec<AnotherCustomerCodec, object>("application/xml", "specific");

            WhenFindingCodec("application/xml", typeof(Customer));

            this.ThenTheResult.CodecType.ShouldBe<AnotherCustomerCodec>();
            this.ThenTheResult.Configuration.ShouldBe("specific");
        }

        [Test]
        public void the_most_specific_codec_for_a_resource_type_is_found()
        {
            GivenACodec<CustomerCodec, object>("application/xml", "nonspecific");
            GivenACodec<CustomerCodec, Customer>("application/xml", "specific");

            WhenFindingCodec("application/xml", typeof(Customer));

            this.ThenTheResult.CodecType.ShouldBe<CustomerCodec>();
            this.ThenTheResult.Configuration.ShouldBe("specific");
        }

        [Test, Ignore]
        public void the_most_specific_codec_is_found_when_matching_against_several_resource_types()
        {
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
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion