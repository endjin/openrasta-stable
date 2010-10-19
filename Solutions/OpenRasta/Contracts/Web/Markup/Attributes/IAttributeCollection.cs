namespace OpenRasta.Contracts.Web.Markup.Attributes
{
    using System;
    using System.Collections.Generic;

    public interface IAttributeCollection : IList<IAttribute>
    {
        IDictionary<string, Func<IAttribute>> AllowedAttributes { get; set; }

        IAttribute this[string key]
        {
            get; set;
        }

        string GetAttribute(string attributeName);

        T GetAttribute<T>(string attributeName);

        void SetAttribute<T>(string attributeName, T value);
    }
}