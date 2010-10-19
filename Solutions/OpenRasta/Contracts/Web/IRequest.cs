namespace OpenRasta.Contracts.Web
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;

    #endregion

    public interface IRequest : IHttpMessage
    {
        IList<string> CodecParameters { get; }

        /// <summary>
        /// Gets or sets the HTTP method used.
        /// </summary>
        string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the culture in which the resource has been requested by the client.
        /// </summary>
        CultureInfo NegotiatedCulture { get; set; }

        /// <summary>
        /// Gets or sets the request Uri
        /// </summary>
        Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the requested URI
        /// </summary>
        string UriName { get; set; }
    }
}