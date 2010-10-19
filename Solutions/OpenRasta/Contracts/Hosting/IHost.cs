namespace OpenRasta.Contracts.Hosting
{
    using System;

    using OpenRasta.Contracts.DI;
    using OpenRasta.Hosting;

    public interface IHost
    {
        event EventHandler Start;

        event EventHandler Stop;

        event EventHandler<IncomingRequestReceivedEventArgs> IncomingRequestReceived;

        event EventHandler<IncomingRequestProcessedEventArgs> IncomingRequestProcessed;

        string ApplicationVirtualPath { get; }

        IDependencyResolverAccessor ResolverAccessor { get; }

        bool ConfigureRootDependencies(IDependencyResolver resolver);

        bool ConfigureLeafDependencies(IDependencyResolver resolver);
    }
}