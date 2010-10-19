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

namespace OpenRasta.Tests.Unit.Codecs.MediaTypeDictionary
{
    #region Using Directives

    using System.Linq;

    using NUnit.Framework;

    using OpenRasta.Testing.Specifications;

    #endregion

    public class when_matching_media_types : media_type_context
    {
        [Test]
        public void registering_a_specific_mediatype_and_matching_on_that_mediatype_returns_one_result()
        {
            GivenMediaType("application/xml", "xml");
            GivenMediaType("text/plain", "text");

            WhenMatching("application/xml");

            this.ThenTheResult.ShouldContain("xml");
            this.ThenTheResult.Count.ShouldBe(1);
        }

        [Test]
        public void registering_a_specific_media_type_and_matching_on_sub_type_wildcard_returns_two_results()
        {
            GivenMediaType("application/xml", "xml");
            GivenMediaType("application/xhtml+xml", "xhtml");
            GivenMediaType("text/plain", "text");

            WhenMatching("application/*");

            this.ThenTheResult.Count.ShouldBe(2);
            this.ThenTheResult.ShouldContain("xhtml");
            this.ThenTheResult.ShouldContain("xml");
        }

        [Test]
        public void matching_on_wildcard_returns_all_results()
        {
            GivenMediaType("application/xml", "xml");
            GivenMediaType("application/xhtml+xml", "xhtml");

            WhenMatching("*/*");

            this.ThenTheResult.Count.ShouldBe(2);
            this.ThenTheResult.ShouldContain("xhtml");
            this.ThenTheResult.ShouldContain("xml");
        }

        [Test]
        public void registering_two_media_types_with_different_values_is_supported()
        {
            GivenMediaType("text/plain", "text1");
            GivenMediaType("text/plain", "text2");

            WhenMatching("text/plain");

            this.ThenTheResult.Count.ShouldBe(2);
            this.ThenTheResult.ShouldContain("text1");
            this.ThenTheResult.ShouldContain("text2");
        }

        [Test]
        public void registering_the_same_media_type_and_associated_value_adds_it_only_once()
        {
            GivenMediaType("text/plain", "text1");
            GivenMediaType("text/plain", "text1");

            WhenMatching("text/plain");

            this.ThenTheResult.Count.ShouldBe(1);
            this.ThenTheResult.ShouldContain("text1");

        }

        [Test]
        public void an_item_with_a_wildcard_media_type_matches_any_media_type()
        {
            GivenMediaType("*/*","wildcard");

            WhenMatching("text/plain");

            this.ThenTheResult.ShouldContain("wildcard").Count().ShouldBe(1);
        }
    }
}