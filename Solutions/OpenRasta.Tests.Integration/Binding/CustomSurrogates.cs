using System;
using System.Net;
using System.Text;
using NUnit.Framework;
using OpenRasta.Configuration;
using OpenRasta.DI;
using OpenRasta.Tests.Integration;
using OpenRasta.TypeSystem;
using OpenRasta.Web;

namespace Dynamic_surrogates
{
    using OpenRasta.TypeSystem.Surrogates;

    public class adding_custom_surrogate : context.surrogates_context
    {

        [Test, Ignore("not implemented yet.")]
        public void surrogate_is_used()
        {
            given_request("GET", "/customer/3");

            when_reading_response();

        }
    }

    public class MySurrogate : AbstractStaticSurrogate<Customer>
    {
        public int Id
        {
            get { return 0; }
            set{throw new InvalidOperationException();}
        }
    }

    namespace context
    {
        using OpenRasta.Configuration.Fluent;
        using OpenRasta.Contracts.TypeSystem.Surrogates;

        using HasExtensions = OpenRasta.Configuration.Extensions.HasExtensions;
        using UsesExtensions = OpenRasta.Configuration.Extensions.UsesExtensions;

        public abstract class surrogates_context : server_context
        {

            public surrogates_context()
        {
            ConfigureServer(() =>
            {

                HasExtensions.ResourcesOfType<Customer>(ResourceSpace.Has)
                    .AtUri("/customer/{id}")
                    .HandledBy<Handler>();

                UsesExtensions.CustomDependency<ISurrogateBuilder, MySurrogate>(ResourceSpace.Uses, DependencyLifetime.Transient);
            });
        }
        }
    }
    
    public class Handler
    {
        public OperationResult Get(Customer customer)
        {
            return new OperationResult.OK();
        }

    }
}