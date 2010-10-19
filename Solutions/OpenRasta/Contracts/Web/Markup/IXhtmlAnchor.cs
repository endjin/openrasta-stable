namespace OpenRasta.Contracts.Web.Markup
{
    #region Using Directives

    using System.ComponentModel;
    using System.Security.Principal;

    using OpenRasta.Contracts.Configuration.Fluent;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Web.Markup.Rendering;

    #endregion

    public interface IXhtmlAnchor : IDependencyResolverAccessor, INoIzObject
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        IXhtmlWriter AmbientWriter { get; }

        IUriResolver Uris { get; }

        IPrincipal User { get; }
    }
}