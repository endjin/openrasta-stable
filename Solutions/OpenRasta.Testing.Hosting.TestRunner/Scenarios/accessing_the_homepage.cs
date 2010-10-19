namespace OpenRasta.Testing.Hosting.TestRunner.Scenarios
{
    using OpenRasta.Testing.Hosting.TestRunner.Infrastructure;
    using OpenRasta.Web;

    /// <summary>
    /// As a user-agent, I want to access the homepage so I can get links to various parts of the application
    /// </summary>
    public class accessing_the_homepage : environment_context
    {
        public void the_homepage_can_be_retrieved_using_xml()
        {
            given_request_to("/")
                .Get()
                .Accept(MediaType.Xml);

            when_retrieving_the_response();

            Response.StatusCode.ShouldBe(200);
            Response.Entity.ContentType.ShouldBe(MediaType.Xml);
            Response.Entity.ContentLength.Value.ShouldBeGreaterThan(0);
        }
    }
}