namespace OpenRasta.Hosting.HttpListener
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using OpenRasta.DI;
    using OpenRasta.Pipeline;

    public class HttpListenerHost : MarshalByRefObject, IHost, IDisposable
    {
        private bool isDisposed;
        private HttpListener listener;
        private IDependencyResolverAccessor resolverAccessor;
        private Type resolverFactory;

        ~HttpListenerHost()
        {
            this.Dispose(false);
        }

        public event EventHandler<IncomingRequestProcessedEventArgs> IncomingRequestProcessed = (s, e) => { };

        public event EventHandler<IncomingRequestReceivedEventArgs> IncomingRequestReceived = (s, e) => { };

        public event EventHandler Start = (s, e) => { };

        public event EventHandler Stop = (s, e) => { };

        public string ApplicationVirtualPath { get; private set; }

        public IDependencyResolverAccessor ResolverAccessor
        {
            get
            {
                if (this.resolverFactory != null && this.resolverAccessor == null)
                {
                    this.resolverAccessor = (IDependencyResolverAccessor)Activator.CreateInstance(this.resolverFactory);
                }

                return this.resolverAccessor;
            }
        }

        public void Close()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Initialize(IEnumerable<string> prefixes, string appPathVDir, Type dependencyResolverFactory)
        {
            this.CheckNotDisposed();
            this.ApplicationVirtualPath = appPathVDir;

            this.resolverFactory = dependencyResolverFactory;
            this.listener = new HttpListener();
            
            foreach (string prefix in prefixes)
            {
                this.listener.Prefixes.Add(prefix);
            }

            HostManager.RegisterHost(this);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void ProcessRequest(IAsyncResult result)
        {
            if (this.isDisposed)
            {
                return;
            }

            HttpListenerContext nativeContext;
            
            try
            {
                nativeContext = this.listener.EndGetContext(result);
            }
            catch (HttpListenerException)
            {
                return;
            }
            
            this.QueueNextRequestPending();
            
            var ambientContext = new AmbientContext();
            var context = new HttpListenerCommunicationContext(this, nativeContext);
            
            try
            {
                using (new ContextScope(ambientContext))
                {
                    this.IncomingRequestReceived(this, new IncomingRequestReceivedEventArgs(context));
                }
            }
            finally
            {
                using (new ContextScope(ambientContext))
                {
                    this.IncomingRequestProcessed(this, new IncomingRequestProcessedEventArgs(context));
                }
            }
        }

        public void StartListening()
        {
            this.CheckNotDisposed();
            
            using (new ContextScope(new AmbientContext()))
            {
                this.Start(this, EventArgs.Empty);
            }

            this.listener.Start();
            this.QueueNextRequestPending();
        }

        public void StopListening()
        {
            this.CheckNotDisposed();

            using (new ContextScope(new AmbientContext()))
            {
                this.Stop(this, EventArgs.Empty);
            }

            this.listener.Stop();
        }

        void IDisposable.Dispose()
        {
            this.Close();
        }

        public virtual bool ConfigureLeafDependencies(IDependencyResolver resolver)
        {
            return true;
        }

        public virtual bool ConfigureRootDependencies(IDependencyResolver resolver)
        {
            resolver.AddDependency<IContextStore, AmbientContextStore>(DependencyLifetime.Singleton);
            
            return true;
        }

        protected virtual void Dispose(bool fromDisposeMethod)
        {
            if (!this.isDisposed)
            {
                if (fromDisposeMethod)
                {
                    if (this.listener.IsListening)
                    {
                        this.StopListening();
                    }

                    HostManager.UnregisterHost(this);
                }

                this.listener.Abort();
                this.isDisposed = true;
            }
        }

        private void CheckNotDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("HttpListenerHost");
            }
        }

        private void QueueNextRequestPending()
        {
            this.listener.BeginGetContext(this.ProcessRequest, null);
        }
    }
}