namespace OpenRasta.Web.UriTemplates
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;

    using OpenRasta.Collections;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Extensions;
    using OpenRasta.TypeSystem;
    using OpenRasta.Web;

    #endregion

    public class TemplatedUriResolver : IUriResolver, IUriTemplateParser
    {
        private UriTemplateTable templates = new UriTemplateTable();

        public TemplatedUriResolver()
        {
            this.TypeSystem = TypeSystems.Default;
        }

        public int Count
        {
            get { return this.templates.KeyValuePairs.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.templates.IsReadOnly; }
        }

        public ITypeSystem TypeSystem { get; set; }

        public void Add(UriRegistration registration)
        {
            if (this.templates.IsReadOnly)
            {
                throw new InvalidOperationException("Cannot add a Uri mapping once the configuration has been done.");
            }

            var resourceKey = this.EnsureIsNotType(registration.ResourceKey);
            
            var descriptor = new UrlDescriptor
            {
                Uri = new UriTemplate(registration.UriTemplate), 
                Culture = registration.UriCulture, 
                ResourceKey = resourceKey, 
                UriName = registration.UriName, 
                Registration = registration
            };
            
            this.templates.KeyValuePairs.Add(new KeyValuePair<UriTemplate, object>(descriptor.Uri, descriptor));
            this.templates.BaseAddress = new Uri("http://localhost/").IgnoreAuthority();
        }

        public void Clear()
        {
            this.templates = new UriTemplateTable();
        }

        public bool Contains(UriRegistration item)
        {
            return this.Any(x => x == item);
        }

        public void CopyTo(UriRegistration[] array, int arrayIndex)
        {
            this.ToList().CopyTo(array, arrayIndex);
        }

        public bool Remove(UriRegistration item)
        {
            var pairToRemove = this.templates.KeyValuePairs
                .Where(x => ((UrlDescriptor)x.Value).Registration == item)
                .ToList();

            if (pairToRemove.Count > 0)
            {
                this.templates.KeyValuePairs.Remove(pairToRemove[0]);
                
                return true;
            }

            return false;
        }

        public IEnumerator<UriRegistration> GetEnumerator()
        {
            return this.templates.KeyValuePairs.Select(x => ((UrlDescriptor)x.Value).Registration).GetEnumerator();
        }

        public Uri CreateUriFor(Uri baseAddress, object resourceKey, string uriName, NameValueCollection keyValues)
        {
            resourceKey = this.EnsureIsNotType(resourceKey);
            var template = this.FindBestMatchingTemplate(this.templates, resourceKey, uriName, keyValues);

            if (template == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "No suitable Uri could be found for resource with key {0} with values {1}.", 
                        resourceKey, 
                        keyValues.ToHtmlFormEncoding()));
            }

            return template.BindByName(baseAddress, keyValues);
        }

        public UriRegistration Match(Uri uriToMatch)
        {
            if (uriToMatch == null)
            {
                return null;
            }

            var tableMatches = this.templates.Match(uriToMatch.IgnoreSchemePortAndAuthority());
            
            if (tableMatches == null || tableMatches.Count == 0)
            {
                return null;
            }

            var urlDescriptor = (UrlDescriptor)tableMatches[0].Data;
            var result = new UriRegistration(urlDescriptor.Uri.ToString(), urlDescriptor.ResourceKey, urlDescriptor.UriName, urlDescriptor.Culture);
            
            foreach (var tableMatch in tableMatches)
            {
                var allVariables = new NameValueCollection
                {
                    tableMatch.BoundVariables, 
                    tableMatch.QueryParameters
                };
                
                result.UriTemplateParameters.Add(allVariables);
            }

            return result;
        }

        public IEnumerable<string> GetQueryParameterNamesFor(string uriTemplate)
        {
            return new UriTemplate(uriTemplate).QueryValueVariableNames;
        }

        public IEnumerable<string> GetTemplateParameterNamesFor(string uriTemplate)
        {
            return new UriTemplate(uriTemplate).PathSegmentVariableNames;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static bool UriNameMatches(string requestUriName, string templateUriName)
        {
            return (!requestUriName.IsNullOrEmpty() &&
                    requestUriName.EqualsOrdinalIgnoreCase(templateUriName)) ||
                   (requestUriName.IsNullOrEmpty() &&
                    templateUriName.IsNullOrEmpty());
        }

        private static bool CompatibleKeys(object requestResourceKey, object templateResourceKey)
        {
            var requestType = requestResourceKey as IType;
            var templateType = templateResourceKey as IType;
            
            return (requestType != null &&
                    templateType != null &&
                    requestType.IsAssignableTo(templateType)) ||
                    requestResourceKey.Equals(templateResourceKey);
        }

        private object EnsureIsNotType(object resourceKey)
        {
            var resourceType = resourceKey as Type;
            
            if (resourceType != null)
            {
                resourceKey = this.TypeSystem.FromClr(resourceType);
            }

            return resourceKey;
        }

        private UriTemplate FindBestMatchingTemplate(UriTemplateTable templates, object resourceKey, string uriName, NameValueCollection keyValues)
        {
            resourceKey = this.EnsureIsNotType(resourceKey);

            var matchingTemplates =
                from template in templates.KeyValuePairs
                let descriptor = (UrlDescriptor)template.Value
                where CompatibleKeys(resourceKey, descriptor.ResourceKey)
                where UriNameMatches(uriName, descriptor.UriName)
                let templateParameters =
                    template.Key.PathSegmentVariableNames.Concat(template.Key.QueryValueVariableNames).ToList()
                let hasKeys = keyValues != null && keyValues.HasKeys()
                where (templateParameters.Count == 0) ||
                      (templateParameters.Count > 0
                       && hasKeys
                       && templateParameters.All(x => keyValues.AllKeys.Contains(x, StringComparison.OrdinalIgnoreCase)))
                orderby templateParameters.Count descending
                select template.Key;

            return matchingTemplates.FirstOrDefault();
        }

        private class UrlDescriptor
        {
            public CultureInfo Culture { get; set; }

            public UriRegistration Registration { get; set; }

            public object ResourceKey { get; set; }
            
            public UriTemplate Uri { get; set; }
            
            public string UriName { get; set; }
        }
    }
}