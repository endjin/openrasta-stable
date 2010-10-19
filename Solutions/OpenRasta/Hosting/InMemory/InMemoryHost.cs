namespace OpenRasta.Hosting.InMemory
{
    using System;

    using OpenRasta.Configuration;
    using OpenRasta.Contracts.Configuration;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Hosting;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;
    using OpenRasta.Extensions;
    using OpenRasta.Pipeline;
    using OpenRasta.Web;

    public class InMemoryHost : IHost, IDependencyResolverAccessor, IDisposable
    {
        private readonly IConfigurationSource configuration;
        private bool isDisposed;

        public InMemoryHost(IConfigurationSource configuration)
        {
            this.configuration = configuration;
            this.Resolver = new InternalDependencyResolver();
            this.ApplicationVirtualPath = "/";
            HostManager = HostManager.RegisterHost(this);
            this.RaiseStart();
        }

        public event EventHandler<IncomingRequestProcessedEventArgs> IncomingRequestProcessed;

        public event EventHandler<IncomingRequestReceivedEventArgs> IncomingRequestReceived;

        public event EventHandler Start;

        public event EventHandler Stop;

        public string ApplicationVirtualPath { get; set; }

        public HostManager HostManager { get; private set; }

        public IDependencyResolver Resolver { get; private set; }

        IDependencyResolverAccessor IHost.ResolverAccessor
        {
            get { return this; }
        }

        public void Close()
        {
            this.RaiseStop();
            HostManager.UnregisterHost(this);
            this.isDisposed = true;
        }

        public IResponse ProcessRequest(IRequest request)
        {
            this.CheckNotDisposed();
            var ambientContext = new AmbientContext();
            var context = new InMemoryCommunicationContext
            {
                ApplicationBaseUri = new Uri("http://localhost"), 
                Request = request, 
                Response = new InMemoryResponse()
            };

            try
            {
                using (new ContextScope(ambientContext))
                {
                    this.RaiseIncomingRequestReceived(context);
                }
            }
            finally
            {
                using (new ContextScope(ambientContext))
                {
                    this.RaiseIncomingRequestProcessed(context);
                }
            }

            return context.Response;
        }

        void IDisposable.Dispose()
        {
            this.Close();
        }

        bool IHost.ConfigureLeafDependencies(IDependencyResolver resolver)
        {
            this.CheckNotDisposed();

            return true;
        }

        bool IHost.ConfigureRootDependencies(IDependencyResolver resolver)
        {
            this.CheckNotDisposed();
            resolver.AddDependencyInstance<IContextStore>(new InMemoryContextStore());

            if (this.configuration != null)
            {
                this.Resolver.AddDependencyInstance<IConfigurationSource>(
                    this.configuration, DependencyLifetime.Singleton);
            }

            return true;
        }

        protected virtual void RaiseIncomingRequestProcessed(ICommunicationContext context)
        {
            this.IncomingRequestProcessed.Raise(this, new IncomingRequestProcessedEventArgs(context));
        }

        protected virtual void RaiseIncomingRequestReceived(ICommunicationContext context)
        {
            this.IncomingRequestReceived.Raise(this, new IncomingRequestReceivedEventArgs(context));
        }

        protected virtual void RaiseStart()
        {
            this.Start.Raise(this, EventArgs.Empty);
        }

        protected virtual void RaiseStop()
        {
            this.Stop.Raise(this, EventArgs.Empty);
        }

        private void CheckNotDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("HttpListenerHost");
            }
        }
    }
}