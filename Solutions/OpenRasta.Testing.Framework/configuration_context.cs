namespace OpenRasta.Testing.Framework
{
    #region Using Directives

    using System;

    using OpenRasta.Configuration;
    using OpenRasta.Configuration.Fluent;
    using OpenRasta.Contracts.Configuration.Fluent;
    using OpenRasta.DI;
    using OpenRasta.Hosting.InMemory;
    using OpenRasta.Testing.Specifications;

    using HasExtensions = OpenRasta.Configuration.Extensions.HasExtensions;

    #endregion

    public class configuration_context : context
    {
        private IDisposable configCookie;
        private InMemoryHost host;
        
        protected override void SetUp()
        {
            base.SetUp();
            this.host = new InMemoryHost(null);
            
            DependencyManager.SetResolver(this.host.Resolver);
            configCookie = OpenRastaConfiguration.Manual;
        }

        protected override void TearDown()
        {
            base.TearDown();
            
            if (configCookie != null)
            {
                configCookie.Dispose();
            }

            this.host.Close();
            
            DependencyManager.UnsetResolver();
        }
        
        public virtual void WhenTheConfigurationIsFinished()
        {
            try
            {
                configCookie.Dispose();
            }
            finally
            {
                configCookie = null;
            }
        }

        public IUriDefinition GivenAResourceRegistrationFor<TResource>(string uri)
        {
            var resourcetype = HasExtensions.ResourcesOfType<TResource>(ResourceSpace.Has);

            return resourcetype.AtUri(uri);
        }

        protected class Customer { }
        
        protected class CustomerHandler { }
    }
}