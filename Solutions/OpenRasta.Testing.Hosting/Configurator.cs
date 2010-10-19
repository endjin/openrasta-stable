namespace OpenRasta.Testing.Hosting
{
    using System.Collections.Generic;

    using OpenRasta.Configuration;
    using OpenRasta.Configuration.Fluent;
    using OpenRasta.Contracts.Authentication;
    using OpenRasta.Contracts.Configuration;
    using OpenRasta.DI;
    using OpenRasta.Testing.Hosting.Handlers;
    using OpenRasta.Testing.Hosting.Resources;

    using HasExtensions = OpenRasta.Configuration.Extensions.HasExtensions;
    using UsesExtensions = OpenRasta.Configuration.Extensions.UsesExtensions;

    public class Configurator : IConfigurationSource
    {
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                UsesExtensions.CustomDependency<IAuthenticationProvider, StaticAuthenticationProvider>(ResourceSpace.Uses, DependencyLifetime.Singleton);
                HasExtensions.ResourcesOfType<Home>(ResourceSpace.Has)
                    .AtUri("/")
                    .HandledBy<HomeHandler>()
                    .AsXmlSerializer();

                /* File upload resources */
                HasExtensions.ResourcesOfType<UploadedFile>(ResourceSpace.Has)
                    .AtUri(Uris.Files)
                    .And.AtUri(Uris.FilesIfile).Named("IFile")
                    .And.AtUri(Uris.FilesComplexType).Named("complexType")
                    .And
                    .AtUri("/files/{id}")
                    .And
                    .AtUri("/files/{fileName}")
                    .HandledBy<UploadedFileHandler>();

                HasExtensions.ResourcesOfType<IEnumerable<User>>(ResourceSpace.Has)
                    .AtUri(Uris.Users)
                    .HandledBy<UserListHandler>()
                    .AsXmlSerializer();

                HasExtensions.ResourcesOfType<User>(ResourceSpace.Has)
                    .AtUri(Uris.USER)
                    .HandledBy<UserHandler>()
                    .AsXmlSerializer();
            }
        }
    }
}