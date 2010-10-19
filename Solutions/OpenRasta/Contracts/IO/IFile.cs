namespace OpenRasta.Contracts.IO
{
    #region Using Directives

    using System.IO;

    using OpenRasta.Web;

    #endregion

    public interface IFile
    {
        MediaType ContentType { get; }

        string FileName { get; }

        long Length { get; }

        Stream OpenStream();
    }
}