namespace OpenRasta.Contracts.IO
{
    public class InMemoryDownloadableFile : InMemoryFile, IDownloadableFile
    {
        public InMemoryDownloadableFile()
        {
            this.Options = DownloadableFileOptions.Save | DownloadableFileOptions.Open;
        }

        public DownloadableFileOptions Options { get; set; }
    }
}