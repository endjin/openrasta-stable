namespace OpenRasta.Configuration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using OpenRasta.Contracts.Configuration.MetaModel;
    using OpenRasta.DI;
    using OpenRasta.Exceptions;

    #endregion

    public static class OpenRastaConfiguration
    {
        private static bool beingConfigured;

        /// <summary>
        /// Creates a manual configuration of the resources supported by the application.
        /// </summary>
        public static IDisposable Manual
        {
            get
            {
                if (beingConfigured)
                {
                    throw new InvalidOperationException("Configuration is already happening on another thread.");
                }

                beingConfigured = true;

                return new FluentConfigurator();
            }
        }

        private static void FinishConfiguration()
        {
            if (!beingConfigured)
            {
                throw new InvalidOperationException(
                    "Something went horribly wrong and the Configuration is deemed finish when it didn't even start!");
            }

            DependencyManager.Pipeline.Initialize();
            beingConfigured = false;
        }

        private class FluentConfigurator : IDisposable
        {
            private bool disposed;

            ~FluentConfigurator()
            {
                Debug.Assert(this.disposed, "The FluentConfigurator wasn't disposed properly.");
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
                var exceptions = new List<OpenRastaConfigurationException>();
                
                try
                {
                    var metaModelRepository = DependencyManager.GetService<IMetaModelRepository>();

                    metaModelRepository.Process();
                }
                finally
                {
                    FinishConfiguration();
                    this.disposed = true;
                    
                    if (exceptions.Count > 0)
                    {
                        throw new OpenRastaConfigurationException(exceptions);
                    }
                }
            }
        }
    }
}