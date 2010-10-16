namespace OpenRasta.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    using OpenRasta.Hosting.HttpListener;

    public class AppDomainHost<T> : IDisposable where T : HttpListenerHost
    {
        private readonly IEnumerable<string> prefixes;
        private readonly Type resolver;
        private readonly string virtualDir;

        public AppDomainHost(IEnumerable<string> prefixes, string virtualDir, Type resolver)
        {
            this.prefixes = prefixes;
            this.virtualDir = virtualDir;
            this.resolver = resolver;
        }

        ~AppDomainHost()
        {
            Dispose();
        }

        public AppDomain HostAppDomain { get; set; }

        public bool IsDisposed { get; private set; }

        public T Listener { get; protected set; }

        public void Initialize()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException("The controller has already been disposed.");
            }

            var appDomainSetup = new AppDomainSetup
                {
                    ApplicationBase = AppDomain.CurrentDomain.BaseDirectory, ShadowCopyFiles = "true" 
                };

            this.HostAppDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), AppDomain.CurrentDomain.Evidence, appDomainSetup);

            this.Listener = (T)
                this.HostAppDomain.CreateInstanceAndUnwrap(
                    typeof(T).Assembly.FullName,
                    typeof(T).FullName,
                    false,
                    BindingFlags.Instance | BindingFlags.Public,
                    null,
                    null,
                    CultureInfo.CurrentCulture,
                    null,
                    this.HostAppDomain.Evidence);

            this.Listener.Initialize(this.prefixes, this.virtualDir, this.resolver);
        }

        public void StartListening()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException("The controller has already been disposed.");
            }

            this.Listener.StartListening();
        }

        public void StopListening()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException("The controller has already been disposed.");
            }

            this.Listener.StopListening();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                this.IsDisposed = true;
                try
                {
                    this.Listener.Close();
                    AppDomain.Unload(this.HostAppDomain);
                }
                catch
                {
                }
            }

            if (disposing)
            {
                this.HostAppDomain = null;
            }
        }
    }
}