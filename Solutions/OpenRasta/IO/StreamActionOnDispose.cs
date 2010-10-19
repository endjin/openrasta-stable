namespace OpenRasta.IO
{
    /// <summary>
    /// Defines what happens to a stream when the user of the stream gets disposed.
    /// </summary>
    public enum StreamActionOnDispose
    {
        /// <summary>
        /// The stream is closed when the owner is disposed.
        /// </summary>
        Close,

        /// <summary>
        /// The stream is not closed.
        /// </summary>
        None
    }
}