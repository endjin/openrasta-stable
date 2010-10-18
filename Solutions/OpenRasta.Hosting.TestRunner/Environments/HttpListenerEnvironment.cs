﻿using OpenRasta.Configuration;
using OpenRasta.DI;
using OpenRasta.Hosting.HttpListener;

namespace OpenBastard.Environments
{
    public class HttpListenerEnvironment : HttpWebRequestEnvironment
    {
        HttpListenerHost host;

        public HttpListenerEnvironment() : base(6687)
        {
        }

        public override string Name
        {
            get { return "HttpListener environment"; }
        }

        public override void Dispose()
        {
            this.host.Close();
            this.host = null;
        }

        public override void Initialize()
        {
            this.host = new HttpListenerHostWithConfiguration(new Configurator());
            this.host.Initialize(new[] { "http://+:" + Port + "/" }, "/", null);
            this.host.StartListening();
        }
    }

    public class HttpListenerHostWithConfiguration : HttpListenerHost
    {
        readonly IConfigurationSource configuration;

        public HttpListenerHostWithConfiguration(IConfigurationSource configuration)
        {
            this.configuration = configuration;
        }

        public override bool ConfigureRootDependencies(IDependencyResolver resolver)
        {
            bool result = base.ConfigureRootDependencies(resolver);
            
            if (result && this.configuration != null)
            {
                resolver.AddDependencyInstance<IConfigurationSource>(this.configuration);
            }

            return result;
        }
    }
}