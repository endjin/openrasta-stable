namespace OpenRasta.Testing.Framework.DI
{
    #region Using Directives

    using NUnit.Framework;

    using OpenRasta.DI;

    #endregion

    [TestFixture]
    public class when_registering_dependencies_with_internal_dependency_resolver : when_registering_dependencies
    {
        public override IDependencyResolver CreateResolver()
        {
            return new InternalDependencyResolver();
        }
    }
}