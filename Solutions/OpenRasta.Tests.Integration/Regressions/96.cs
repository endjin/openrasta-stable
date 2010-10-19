using System;
using NUnit.Framework;
using OpenRasta.DI;
using OpenRasta.Hosting.HttpListener;


namespace OpenRasta.Tests.Integration.Regressions
{
    using OpenRasta.Contracts.DI;
    using OpenRasta.Testing.Specifications;

    public class when_httplistenerhost_resolveraccessor_is_accessed_multiple_times : context
    {
        private static readonly int PORT = 6687;

        [Test]
        public void the_results_should_be_the_same_instance()
        {
            var host = new HttpListenerHost();
            host.Initialize(new[] { "http://+:" + PORT + "/" }, "/", typeof(DependencyResolverAccessorStub));
            var instance1 = host.ResolverAccessor;
            var instance2 = host.ResolverAccessor;

            instance1.ShouldBeTheSameInstanceAs(instance2);
        }
    }

    public class DependencyResolverAccessorStub : IDependencyResolverAccessor
    {
        public IDependencyResolver Resolver
        {
            get { return null; }
        }
    }
}