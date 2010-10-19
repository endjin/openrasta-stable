namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    /// <summary>
    /// Represents a boolean sgmml attribute, of the form attrib="attrib"
    /// </summary>
    public class BooleanAttribute : XhtmlAttributeCore
    {
        public BooleanAttribute()
        {
        }

        public BooleanAttribute(string name) : base(name)
        {
        }

        protected override Func<IAttribute> Factory(string propName)
        {
            return () => (IAttribute)new XhtmlAttributeNode<bool>(
                                         propName,
                                         false,
                                         b => b ? propName.ToLowerInvariant() : null,
                                         str => string.Compare(str, propName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                DefaultValue = null
            };
        }
    }
}