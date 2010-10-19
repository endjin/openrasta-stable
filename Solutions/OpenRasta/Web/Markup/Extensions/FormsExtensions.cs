namespace OpenRasta.Web.Markup.Extensions
{
    #region Using Directives

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Contracts.Web.UriDecorators;
    using OpenRasta.Web.Markup.Controls;
    using OpenRasta.Web.Markup.Modules;
    using OpenRasta.Web.UriDecorators;

    #endregion

    public static class FormsExtensions
    {
        public static IFormElement Form(this IXhtmlAnchor anchor, object resourceInstance)
        {
            return new FormElement(IsUriMethodOverrideActive(anchor.Resolver)).Action(resourceInstance.CreateUri());
        }

        public static IFormElement Form<TResource>(this IXhtmlAnchor anchor)
        {
            return new FormElement(IsUriMethodOverrideActive(anchor.Resolver)).Action(anchor.Uris.CreateUriFor<TResource>());
        }
        
        public static IAElement Link<T>(this IXhtmlAnchor anchor)
        {
            return Document.CreateElement<IAElement>().Href(anchor.Uris.CreateUriFor<T>());
        }
        
        public static IAElement Link(this IXhtmlAnchor anchor, object instance)
        {
            return Document.CreateElement<IAElement>().Href(instance.CreateUri());
        }

        private static bool IsUriMethodOverrideActive(IDependencyResolver resolver)
        {
            return resolver.HasDependencyImplementation(typeof(IUriDecorator), typeof(HttpMethodOverrideUriDecorator));
        }
    }
}