namespace OpenRasta.Hosting.AspNet.AspNetHttpListener
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Web;
    using System.Web.Hosting;

    using OpenRasta.Configuration;
    using OpenRasta.Contracts.Configuration;

    // Warning, this class will undergo massive refactorings sooner or later, don't rely on it.
    public class HttpListenerAspNetHost : MarshalByRefObject
    {
        private System.Net.HttpListener listener;
        private string physicalDir;
        private string virtualDir;

        public void Configure(string[] prefixes, string vdir, string pdir)
        {
            this.virtualDir = vdir;
            this.physicalDir = pdir;
            this.listener = new System.Net.HttpListener();

            foreach (string prefix in prefixes)
            {
                this.listener.Prefixes.Add(prefix);
            }
        }

        public void ExecuteConfig(Action t)
        {
            ((Config)OpenRastaModule.Host.ConfigurationSource).ConfigurationLambda = () =>
                {
                    using (OpenRastaConfiguration.Manual)
                    {
                        t();
                    }
                };
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void ProcessRequest()
        {
            HttpListenerContext ctx;
            try
            {
                ctx = this.listener.GetContext();
            }
            catch (HttpListenerException)
            {
                return;
            }

            this.QueueNextRequestWait();
            var workerRequest = new HttpListenerWorkerRequest(ctx, this.virtualDir, this.physicalDir);
            
            try
            {
                HttpRuntime.ProcessRequest(workerRequest);
            }
            catch
            {
            }
        }

        public void Start()
        {
            OpenRastaModule.Host.ConfigurationSource = new Config();
            this.listener.Start();
            this.QueueNextRequestWait();
        }

        public void Stop()
        {
            this.listener.Stop();
            ApplicationManager.GetApplicationManager().ShutdownAll();
        }

        void QueueNextRequestWait()
        {
            ThreadPool.QueueUserWorkItem(s => this.ProcessRequest());
        }

        class Config : IConfigurationSource
        {
            public Action ConfigurationLambda;

            public void Configure()
            {
                this.ConfigurationLambda();
            }
        }
    }
}