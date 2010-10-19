namespace OpenRasta.Contracts.Web.UriDecorators
{
    using System;

    /// <summary>
    /// Defines a decoration on a url, used to modify the context of the request and reshape a url before processing.
    /// </summary>
    public interface IUriDecorator
    {
        /// <summary>
        /// Applies any changes after al the Uri decorators have been called.
        /// </summary>
        void Apply();

        /// <summary>
        /// Parses a Uri.
        /// </summary>
        /// <param name="uri">The uri to process</param>
        /// <param name="processedUri">The uri after processing</param>
        /// <returns>true if work needs to be done after the Uri processing is done, resulting in a call to Apply(). Otherwise false.</returns>
        bool Parse(Uri uri, out Uri processedUri);
    }
}