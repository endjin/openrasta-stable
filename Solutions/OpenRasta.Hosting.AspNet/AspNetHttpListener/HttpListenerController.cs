namespace OpenRasta.Hosting.AspNet.AspNetHttpListener
{
    using System.Web.Hosting;

    // code originally based on xml-rpc.net under MIT license
    // see http://code.google.com/p/xmlrpcnet/
    public class HttpListenerController
    {
        private readonly string physicalDir;
        private readonly string[] prefixes;
        private readonly string virtualDir;

        public HttpListenerController(string[] prefixes, string vdir, string pdir)
        {
            this.prefixes = prefixes;
            this.virtualDir = vdir;
            this.physicalDir = pdir;
        }

        public HttpListenerAspNetHost Host { get; private set; }

        public void Start()
        {
            this.Host = (HttpListenerAspNetHost)ApplicationHost.CreateApplicationHost(typeof(HttpListenerAspNetHost), this.virtualDir, this.physicalDir);

            this.Host.Configure(this.prefixes, this.virtualDir, this.physicalDir);
            this.Host.Start();
        }

        public void Stop()
        {
            this.Host.Stop();
        }
    }
}