namespace OpenRasta.Contracts.Web
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using OpenRasta.Web;

    #endregion

    public interface IUriResolver : ICollection<UriRegistration>
    {
        UriRegistration Match(Uri uriToMatch);

        Uri CreateUriFor(Uri baseAddress, object resourceKey, string uriName, NameValueCollection keyValues);
    }
}