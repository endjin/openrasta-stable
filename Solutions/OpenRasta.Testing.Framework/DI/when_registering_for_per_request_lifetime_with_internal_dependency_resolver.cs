namespace OpenRasta.Testing.Framework.DI
{
    #region Using Directives

    using NUnit.Framework;

    using OpenRasta.Contracts.DI;
    using OpenRasta.DI;

    #endregion

    [TestFixture]
    public class when_registering_for_per_request_lifetime_with_internal_dependency_resolver : when_registering_for_per_request_lifetime
    {
        public override IDependencyResolver CreateResolver()
        {
            return new InternalDependencyResolver();
        }
    }
}