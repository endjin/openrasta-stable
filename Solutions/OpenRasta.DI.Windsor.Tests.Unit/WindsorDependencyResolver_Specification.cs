namespace WindsorDependencyResolver_Specification
{
    #region Using Directives

    using Castle.Windsor;

    using NUnit.Framework;

    using OpenRasta.Contracts.DI;
    using OpenRasta.DI.Windsor;
    using OpenRasta.Testing.Framework.DI;

    #endregion

    [TestFixture]
    public class when_resolving_instances_the_castle_resolver : when_resolving_instances
    {
        public override IDependencyResolver CreateResolver()
        {
            return new WindsorDependencyResolver(new WindsorContainer());
        }
    }

    [TestFixture]
    public class when_registering_dependencies_with_the_castle_resolver : when_registering_dependencies
    {
        public override IDependencyResolver CreateResolver()
        {
            return new WindsorDependencyResolver(new WindsorContainer());
        }
    }

    [TestFixture]
    public class when_registering_for_per_request_lifetime_with_internal_dependency_resolver : when_registering_for_per_request_lifetime
    {
        public override IDependencyResolver CreateResolver()
        {
            return new WindsorDependencyResolver(new WindsorContainer());
        }
    }
}