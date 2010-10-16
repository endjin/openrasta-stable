namespace OpenRasta.Hosting
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.Configuration;
    using OpenRasta.DI;
    using OpenRasta.Diagnostics;
    using OpenRasta.Pipeline;
    using OpenRasta.Web;

    public class HostManager : IDisposable
    {
        private static readonly IDictionary<IHost, HostManager> Registrations = new Dictionary<IHost, HostManager>();
        private readonly object syncRoot = new object();

        static HostManager()
        {
            Log = new TraceSourceLogger();
        }

        private HostManager(IHost host)
        {
            this.Host = host;
            this.Host.Start += this.HandleHostStart;
            this.Host.IncomingRequestReceived += this.HandleHostIncomingRequestReceived;
            this.Host.IncomingRequestProcessed += this.HandleIncomingRequestProcessed;
        }

        public IHost Host { get; private set; }

        public bool IsConfigured { get; private set; }

        public IDependencyResolver Resolver { get; private set; }

        private static ILogger Log { get; set; }

        public static HostManager RegisterHost(IHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }

            Log.WriteInfo("Registering host of type {0}", host.GetType());

            var manager = new HostManager(host);

            lock (Registrations)
            {
                Registrations.Add(host, manager);
            }

            return manager;
        }

        public static void UnregisterHost(IHost host)
        {
            Log.WriteInfo("Unregistering host of type {0}", host.GetType());
            HostManager managerToDispose = null;
            
            lock (Registrations)
            {
                if (Registrations.ContainsKey(host))
                {
                    managerToDispose = Registrations[host];
                    Registrations.Remove(host);
                }
            }

            if (managerToDispose != null)
            {
                managerToDispose.Dispose();
            }
        }

        public void InvalidateConfiguration()
        {
            lock (this.syncRoot)
            {
                this.IsConfigured = false;
            }
        }

        public void SetupCommunicationContext(ICommunicationContext context)
        {
            Log.WriteDebug("Adding communication context data");

            this.Resolver.AddDependencyInstance<ICommunicationContext>(context, DependencyLifetime.PerRequest);
            this.Resolver.AddDependencyInstance<IRequest>(context.Request, DependencyLifetime.PerRequest);
            this.Resolver.AddDependencyInstance<IResponse>(context.Response, DependencyLifetime.PerRequest);
        }

        public void Dispose()
        {
            this.Host.Start -= this.HandleHostStart;
            this.Host.IncomingRequestReceived -= this.HandleHostIncomingRequestReceived;
            this.Host.IncomingRequestProcessed -= this.HandleIncomingRequestProcessed;
        }

        private void AssignResolver()
        {
            this.Resolver = this.Host.ResolverAccessor != null
                           ? this.Host.ResolverAccessor.Resolver
                           : new InternalDependencyResolver();
            if (!this.Resolver.HasDependency<IDependencyResolver>())
            {
                this.Resolver.AddDependencyInstance(typeof(IDependencyResolver), this.Resolver);
            }

            Log.WriteDebug("Using dependency resolver of type {0}", this.Resolver.GetType());
        }

        private void Configure()
        {
            this.IsConfigured = false;
            this.AssignResolver();

            this.ThreadScopedAction(() =>
            {
                RegisterRootDependencies();

                VerifyContextStoreRegistered();

                RegisterCoreDependencies();

                RegisterLeafDependencies();

                ExecuteConfigurationSource();

                IsConfigured = true;
            });
        }

        private void ExecuteConfigurationSource()
        {
            if (this.Resolver.HasDependency<IConfigurationSource>())
            {
                var configSource = this.Resolver.Resolve<IConfigurationSource>();
                Log.WriteDebug("Using configuration source {0}", configSource.GetType());
                configSource.Configure();
            }
            else
            {
                Log.WriteDebug("Not using any configuration source.");
            }
        }

        private void RegisterCoreDependencies()
        {
            var registrar = this.Resolver.ResolveWithDefault<IDependencyRegistrar>(() => new DefaultDependencyRegistrar());
            Log.WriteInfo("Using dependency registrar of type {0}.", registrar.GetType());
            registrar.Register(this.Resolver);
        }

        private void RegisterLeafDependencies()
        {
            Log.WriteDebug("Registering host's leaf dependencies.");
            if (!this.Host.ConfigureLeafDependencies(this.Resolver))
            {
                throw new OpenRastaConfigurationException("Leaf dependencies configuration by host has failed.");
            }
        }

        private void RegisterRootDependencies()
        {
            Log.WriteDebug("Registering host's root dependencies.");
            if (!this.Host.ConfigureRootDependencies(this.Resolver))
            {
                throw new OpenRastaConfigurationException("Root dependencies configuration by host has failed.");
            }
        }

        private void ThreadScopedAction(Action action)
        {
            bool resolverSet = false;
            try
            {
                DependencyManager.SetResolver(this.Resolver);
                resolverSet = true;
                action();
            }
            finally
            {
                if (resolverSet)
                {
                    DependencyManager.UnsetResolver();
                }
            }
        }

        private void VerifyConfiguration()
        {
            if (!this.IsConfigured)
            {
                lock (this.syncRoot)
                {
                    if (!this.IsConfigured)
                    {
                        this.Configure();
                    }
                }
            }
        }

        private void VerifyContextStoreRegistered()
        {
            if (!this.Resolver.HasDependency<IContextStore>())
            {
                throw new OpenRastaConfigurationException("The host didn't register a context store.");
            }
        }

        protected virtual void HandleHostIncomingRequestReceived(object sender, IncomingRequestEventArgs e)
        {
            this.VerifyConfiguration();
            Log.WriteDebug("Incoming host request for " + e.Context.Request.Uri);
            this.ThreadScopedAction(() =>
            {
                // register the required dependency in the web context
                var context = e.Context;
                SetupCommunicationContext(context);
                Resolver.AddDependencyInstance<IHost>(Host, DependencyLifetime.PerRequest);

                Resolver.Resolve<IPipeline>().Run(context);
            });
        }

        protected virtual void HandleHostStart(object sender, EventArgs e)
        {
            this.VerifyConfiguration();
        }

        protected virtual void HandleIncomingRequestProcessed(object sender, IncomingRequestProcessedEventArgs e)
        {
            Log.WriteDebug("Request finished.");
            this.ThreadScopedAction(() => this.Resolver.HandleIncomingRequestProcessed());
        }
    }
}