namespace OpenRasta.Contracts.Web
{
    #region Using Directives

    using System;
    using System.Collections.Specialized;

    using OpenRasta.Collections.Specialized;
    using OpenRasta.DI;

    #endregion

    public static class IUriResolverExtensions
    {
        public static Uri CreateUri(this object target)
        {
            return CreateUri(target, (string)null);
        }

        public static Uri CreateUri(this object target, string uriName)
        {
            return target.CreateUri(uriName, null);
        }
        
        public static Uri CreateUri(this object target, string uriName, object additionalProperties)
        {
            return target.CreateUri(DependencyManager.GetService<ICommunicationContext>().ApplicationBaseUri, uriName, additionalProperties);
        }
        
        public static Uri CreateUri(this object target, object additionalProperties)
        {
            return target.CreateUri((string)null, additionalProperties);
        }

        public static Uri CreateUri(this object target, Uri baseUri)
        {
            return target.CreateUri(baseUri, null);
        }
        
        public static Uri CreateUri(this object target, Uri baseUri, string uriName)
        {
            return target.CreateUri(baseUri, uriName, null);
        }
        
        public static Uri CreateUri(this object target, Uri baseUri, object additionalProperties)
        {
            return target.CreateUri(baseUri, null, additionalProperties);
        }
        
        public static Uri CreateUri(this object target, Uri baseUri, string uriName, object additionalProperties)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            var uriResolver = DependencyManager.GetService<IUriResolver>();
            
            if (baseUri == null)
            {
                baseUri = DependencyManager.GetService<ICommunicationContext>().ApplicationBaseUri;
            }
            
            if (target is Type)
            {
                if (additionalProperties != null)
                {
                    return uriResolver.CreateUriFor(
                        baseUri, ((Type)target), uriName, additionalProperties.ToNameValueCollection());
                }

                return uriResolver.CreateUriFor(baseUri, (Type)target, uriName);
            }

            var props = target.ToNameValueCollection();
            
            return uriResolver.CreateUriFor(baseUri, target.GetType(), uriName, Merge(props, additionalProperties));
        }

        public static Uri CreateUriFor<T>(this IUriResolver resolver)
        {
            return resolver.CreateUriFor(typeof(T));
        }

        public static Uri CreateUriFor(this IUriResolver resolver, Type type)
        {
            return resolver.CreateUriFor(type, null);
        }

        public static Uri CreateUriFor(this IUriResolver resolver, Type type, object keyValues)
        {
            return resolver.CreateUriFor(type, keyValues != null ? keyValues.ToNameValueCollection() : null);
        }

        public static Uri CreateUriFor(this IUriResolver resolver, Type type, NameValueCollection keyValues)
        {
            return resolver.CreateUriFor(type, null, keyValues);
        }

        public static Uri CreateUriFor(this IUriResolver resolver, Type type, string uriName, object keyValues)
        {
            return resolver.CreateUriFor(type, uriName, keyValues != null ? keyValues.ToNameValueCollection() : null);
        }

        public static Uri CreateUriFor(this IUriResolver resolver, Type type, string uriName, NameValueCollection keyValues)
        {
            return resolver.CreateUriFor(
                DependencyManager.GetService<ICommunicationContext>().ApplicationBaseUri, type, uriName, keyValues);
        }

        public static Uri CreateUriFor(this IUriResolver resolver, Uri baseAddress, Type type)
        {
            return resolver.CreateUriFor(baseAddress, type, (string)null);
        }

        public static Uri CreateUriFor(this IUriResolver resolver, Uri baseAddress, Type type, string uriName)
        {
            return resolver.CreateUriFor(baseAddress, type, uriName, (NameValueCollection)null);
        }
        
        public static Uri CreateUriFor(this IUriResolver resolver, Uri baseAddress, Type type, object nameValues)
        {
            return resolver.CreateUriFor(baseAddress, type, nameValues != null ? nameValues.ToNameValueCollection() : null);
        }
        
        public static Uri CreateUriFor(this IUriResolver resolver, Uri baseAddress, Type resourceType, NameValueCollection nameValues)
        {
            return resolver.CreateUriFor(baseAddress, resourceType, string.Empty, nameValues);
        }
        
        private static NameValueCollection Merge(NameValueCollection source, object target)
        {
            if (target == null)
            {
                return source;
            }

            if (source == null)
            {
                source = new NameValueCollection();
            }
            
            if (target is NameValueCollection)
            {
                source.AddReplace((NameValueCollection)target);
            }
            else
            {
                source.AddReplace(target.ToNameValueCollection());
            }

            return source;
        }
    }
}