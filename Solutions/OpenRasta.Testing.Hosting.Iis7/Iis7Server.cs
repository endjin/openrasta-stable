namespace OpenRasta.Testing.Hosting.Iis7
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public class Iis7Server : IDisposable
    {
        private readonly string appHostConfigPath;
        private readonly string rootWebConfigPath;
        private bool disposed;

        public Iis7Server(string physicalPath, int port, int siteId, bool useIntegratedPipeline)
        {
            string appPoolName = "AppPool" + port;
            this.appHostConfigPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".config");
            this.rootWebConfigPath = Environment.ExpandEnvironmentVariables(@"%windir%\Microsoft.Net\Framework\v2.0.50727\config\web.config");

            File.WriteAllText(
                this.appHostConfigPath,
                String.Format(
                IisConfigFiles.applicationHost,
                    port, 
                    physicalPath, 
                    siteId, 
                    appPoolName, 
                    useIntegratedPipeline ? "Integrated" : "Classic"));
        }

        ~Iis7Server()
        {
            this.Dispose(false);
        }

        public void Start()
        {
            this.CheckDisposed();

            HostableWebCore.Activate(this.appHostConfigPath, this.rootWebConfigPath, Guid.NewGuid().ToString());
        }

        public void Stop()
        {
            ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            this.CheckDisposed();
            
            GC.SuppressFinalize(this);
            
            this.Dispose(true);
        }

        private void CheckDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("The server has already been disposed");
            }
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposed = true;
                HostableWebCore.Shutdown(false);
            }
        }

        public static class HostableWebCore
        {
            private static bool isActivated;

            /// <summary>
            /// Specifies if Hostable WebCore ha been activated
            /// </summary>
            public static bool IsActivated
            {
                get { return isActivated; }
            }

            // public void Activate(string appHostConfig, string rootWebConfig, string instanceName){}
            /// <summary>
            /// Activate the HWC
            /// </summary>
            /// <param name="appHostConfig">Path to ApplicationHost.config to use</param>
            /// <param name="rootWebConfig">Path to the Root Web.config to use</param>
            /// <param name="instanceName">Name for this instance</param>
            public static void Activate(string appHostConfig, string rootWebConfig, string instanceName)
            {
                if (isActivated)
                {
                    return;
                }

                int result = HWebCore64.WebCoreActivate(appHostConfig, rootWebConfig, instanceName);
                
                if (result != 0)
                {
                    Marshal.ThrowExceptionForHR(result);
                }

                isActivated = true;
            }

            /// <summary>
            /// Shutdown HWC
            /// </summary>
            public static void Shutdown(bool immediate)
            {
                if (isActivated)
                {
                    int result = HWebCore64.WebCoreShutdown(immediate);
                    
                    if (result != 0)
                    {
                        Marshal.ThrowExceptionForHR(result);
                    }
                    
                    isActivated = false;
                }
            }

            private static class HWebCore64
            {
                [DllImport(@"inetsrv\hwebcore.dll")]
                public static extern int WebCoreActivate(
                    [In, MarshalAs(UnmanagedType.LPWStr)] string appHostConfigPath, 
                    [In, MarshalAs(UnmanagedType.LPWStr)] string rootWebConfigPath, 
                    [In, MarshalAs(UnmanagedType.LPWStr)] string instanceName);

                [DllImport(@"inetsrv\hwebcore.dll")]
                public static extern int WebCoreShutdown(bool immediate);
            }
        }
    }
}