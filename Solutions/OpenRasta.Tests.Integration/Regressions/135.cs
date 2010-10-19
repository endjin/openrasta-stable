using System;
using System.Net;
using NUnit.Framework;
using OpenRasta.Configuration;
using OpenRasta.Pipeline;


namespace OpenRasta.Tests.Integration.Regressions
{
    using OpenRasta.Configuration.Extensions;
    using OpenRasta.Configuration.Fluent;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Testing.Specifications;

    using UsesExtensions = OpenRasta.Configuration.Extensions.UsesExtensions;
    using XmlConfigurationExtensions = OpenRasta.Configuration.Extensions.XmlConfigurationExtensions;

    public class when_pipeline_contributor_raises_exception_after_operation_executed : server_context
    {
        private static readonly int PORT = 6687;

        public when_pipeline_contributor_raises_exception_after_operation_executed()
        {

            ConfigureServer(() =>
            {
                UsesExtensions.PipelineContributor<RecursiveContributor>(ResourceSpace.Uses);
                XmlConfigurationExtensions.AsXmlDataContract(ResourceSpace.Has.ResourcesOfType<string>()
                        .AtUri("/news")
                        .HandledBy<DefaultHandler>());
            });
        }
        [Test]
        public void the_pipeline_doesnt_recurse_and_run_quickly()
        {
            the_pipeline_doesnt_recurse();
        }
        public void the_pipeline_doesnt_recurse()
        {
            given_request("GET", "/news");

            when_reading_response();

            this.TheResponse.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        }
    }

    public class RecursiveContributor : IPipelineContributor
    {
        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(x => { throw new InvalidOperationException(); }).After<KnownStages.IOperationResultInvocation>();
        }
    }

    public class DefaultHandler
    {
        public string Get() {
            return "hello";
        }
    }

}