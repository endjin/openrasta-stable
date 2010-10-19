namespace OpenRasta.Web.Markup
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using OpenRasta.Collections;
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;
    using OpenRasta.Web.Markup.Attributes.Annotations;
    using OpenRasta.Web.Markup.Elements;

    #endregion

    public class Document
    {
        public static T CreateElement<T>() where T : class, IElement
        {
            string tagName = ExtractTagNameFrom<T>();

            if (tagName == null)
            {
                throw new ArgumentException("T");
            }

            return CreateElement<T>(tagName.ToLowerInvariant());
        }

        public static T CreateElement<T>(string elementName) where T : class, IElement
        {
            return (T)CreateElementCore<T>(elementName);
        }

        public static IEnumerable<Type> GetContentModelFor<T>()
        {
            foreach (var interfaceType in typeof(T).GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IContentModel<,>)))
            {
                yield return interfaceType.GetGenericArguments()[1];
            }
        }

        public static IDictionary<string, Func<IAttribute>> GetAllowedAttributesFor<T>()
        {
            var allAttributes = from prop in GetProperties(typeof(T)).Distinct()
                                where prop.CanRead
                                let attribs = Attribute.GetCustomAttributes(prop)
                                where attribs != null && attribs.Length > 0
                                let attrib = attribs.Where(a => typeof(XhtmlAttributeCore).IsAssignableFrom(a.GetType())).FirstOrDefault() as XhtmlAttributeCore
                                where attrib != null
                                select new { prop, attrib };
            return allAttributes.Distinct().ToDictionary(key => key.prop.Name, val => val.attrib.Factory(val.prop), StringComparer.OrdinalIgnoreCase);
        }

        private static IElement CreateElementCore<T>(string elementName) where T : class, IElement
        {
            IEnumerable<Type> contentModels = GetContentModelFor<T>();
            var elementAttribs = GetAllowedAttributesFor<T>();
            
            var genericElemnt = new GenericElement(elementName) { Attributes = { AllowedAttributes = elementAttribs } };

            genericElemnt.ContentModel.AddRange(contentModels);
            
            return genericElemnt;
        }

        private static string ExtractTagNameFrom<T>()
        {
            string typeName = typeof(T).Name;

            if (typeName.StartsWith("I") && typeName.EndsWith("Element"))
            {
                return typeName.Substring(1, typeName.Length - 8);
            }

            return null;
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            foreach (var prop in type.GetProperties())
            {
                yield return prop;
            }

            foreach (var parentType in type.GetInterfaces())
            {
                foreach (var pi in GetProperties(parentType))
                {
                    yield return pi;
                }
            }
        }
    }
}