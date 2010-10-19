namespace OpenRasta.Testing.Hosting.Resources
{
    using OpenRasta.Contracts.IO;
    using OpenRasta.IO;

    public class UploadedFile
    {
        public string Description { get; set; }

        public IFile File { get; set; }
    }
}