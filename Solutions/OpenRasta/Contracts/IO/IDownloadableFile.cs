namespace OpenRasta.Contracts.IO
{
    public interface IDownloadableFile : IFile
    {
        DownloadableFileOptions Options { get; }
    }
}