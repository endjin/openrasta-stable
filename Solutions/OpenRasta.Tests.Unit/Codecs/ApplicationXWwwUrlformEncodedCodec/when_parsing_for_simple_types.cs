﻿#region License

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

namespace OpenRasta.Tests.Unit.Codecs.ApplicationXWwwUrlformEncodedCodec
{
    #region Using Directives

    using NUnit.Framework;

    using OpenRasta.Testing.Specifications;

    #endregion

    public class when_parsing_for_simple_types : app_www_context
    {
        [Test]
        public void url_encoding_is_resolved()
        {
            given_context();
            given_request_stream("thecustomer=John%20Doe");

            when_decoding<string>("thecustomer");

            then_decoding_result<string>().ShouldBe("John Doe");
        }

        [Test]
        public void strings_are_assigned()
        {
            given_context();
            given_request_stream("thecustomer=John&thecustomer=Jack");

            when_decoding<string[]>("thecustomer");

            then_decoding_result<string[]>().ShouldHaveSameElementsAs(new[] { "John", "Jack" });
        }
        
        [Test]
        public void string_arrays_are_assigned()
        {
            given_context();
            given_request_stream("thecustomer=John");

            when_decoding<string>("thecustomer");

            then_decoding_result<string>().ShouldBe("John");
        }
        
        [Test]
        public void integers_are_assigned()
        {
            given_context();
            given_request_stream("thecustomerid=3");

            when_decoding<int>("thecustomerid");

            then_decoding_result<int>().ShouldBe(3);
        }
    }
}