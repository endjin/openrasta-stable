namespace OpenRasta.Web.Markup
{
    #region Using Directives

    using System;
    using System.Security.Principal;

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Contracts.Web.Markup.Rendering;
    using OpenRasta.DI;

    #endregion

    /// <summary>
    /// Marker class used to provide xhtml-related functionality from within pages using extension methods.
    /// </summary>
    public class XhtmlAnchor : IXhtmlAnchor
    {
        private readonly IDependencyResolver resolver;
        private readonly Func<IPrincipal> userGetter;

        public XhtmlAnchor(IDependencyResolver resolver, IXhtmlWriter writer, Func<IPrincipal> userGetter)
        {
            this.resolver = resolver;
            this.userGetter = userGetter;
            this.AmbientWriter = writer;
        }

        public IXhtmlWriter AmbientWriter { get; private set; }

        public IUriResolver Uris
        {
            get { return this.resolver.Resolve<IUriResolver>(); }
        }

        public IPrincipal User
        {
            get { return this.userGetter(); }
        }

        public IDependencyResolver Resolver
        {
            get { return this.resolver; }
        }
    }
}