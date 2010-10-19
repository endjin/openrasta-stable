namespace UriTemplateTable_Specification
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using NUnit.Framework;

    using OpenRasta.Testing.Specifications;
    using OpenRasta.Web.UriTemplates;

    #endregion

    [TestFixture]
    public class when_matching_a_template_table
    {
        [Test]
        public void out_of_two_templates_with_one_query_parameter_only_the_correct_one_is_used()
        {
            var table = new UriTemplateTable(new Uri("http://localhost"), new List<KeyValuePair<UriTemplate, object>>
            {
                new KeyValuePair<UriTemplate, object>(new UriTemplate("resource1?query={queryText}"), null),
                new KeyValuePair<UriTemplate, object>(new UriTemplate("resource1?query2={queryText}"), null)
            });

            Collection<UriTemplateMatch> match = table.Match(new Uri("http://localhost/resource1?query=testing a query"));
            match.Count.ShouldBe(1);
            match[0].QueryParameters["queryText"].ShouldBe("testing a query");
        }
    }
}