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

namespace OpenRasta.Tests.Unit.Codecs.ApplicationXWwwUrlformEncodedCodec
{
    #region Using Directives

    using System.Collections.Generic;

    using NUnit.Framework;

    using OpenRasta.Testing.Specifications;

    #endregion

    public class when_the_requested_type_is_a_dictionary : app_www_context
    {
        [Test]
        public void the_values_are_returned()
        {
            given_context();
            given_request_stream("Customer.Something=John&Customer.SomethingElse=Doe");

            when_decoding<Dictionary<string, string[]>>();

            ThenTheResult
                .ShouldContain("Customer.Something", new[] { "John" })
                .ShouldContain("Customer.SomethingElse", new[] { "Doe" });

        }
        private Dictionary<string, string[]> ThenTheResult { get { return base.then_decoding_result<Dictionary<string, string[]>>(); } }
    }
}