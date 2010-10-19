namespace OpenRasta.Contracts.Web
{
    using System.IO;

    public interface ISupportsTextWriter
    {
        TextWriter TextWriter { get; }
    }
}