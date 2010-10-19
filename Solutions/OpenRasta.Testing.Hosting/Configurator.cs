namespace OpenRasta.Testing.Hosting
{
    using System.Collections.Generic;

    using OpenRasta.Configuration;
    using OpenRasta.DI;
    using OpenRasta.Security;
    using OpenRasta.Testing.Hosting.Handlers;
    using OpenRasta.Testing.Hosting.Resources;

    public class Configurator : IConfigurationSource
    {
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Uses.CustomDependency<IAuthenticationProvider, StaticAuthenticationProvider>(DependencyLifetime.Singleton);
                ResourceSpace.Has.ResourcesOfType<Home>()
                    .AtUri("/")
                    .HandledBy<HomeHandler>()
                    .AsXmlSerializer();

                /* File upload resources */
                ResourceSpace.Has.ResourcesOfType<UploadedFile>()
                    .AtUri(Uris.Files)
                    .And.AtUri(Uris.FilesIfile).Named("IFile")
                    .And.AtUri(Uris.FilesComplexType).Named("complexType")
                    .And
                    .AtUri("/files/{id}")
                    .And
                    .AtUri("/files/{fileName}")
                    .HandledBy<UploadedFileHandler>();

                ResourceSpace.Has.ResourcesOfType<IEnumerable<User>>()
                    .AtUri(Uris.Users)
                    .HandledBy<UserListHandler>()
                    .AsXmlSerializer();

                ResourceSpace.Has.ResourcesOfType<User>()
                    .AtUri(Uris.USER)
                    .HandledBy<UserHandler>()
                    .AsXmlSerializer();
            }
        }
    }
}