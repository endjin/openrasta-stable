namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    public class CDATAAttribute : XhtmlAttributeCore
    {
        public CDATAAttribute()
        {
        }

        public CDATAAttribute(string name) : base(name)
        {
        }

        protected override Func<IAttribute> Factory(string propertyName)
        {
            return () => (IAttribute)new XhtmlAttributeNode<string>(propertyName, false, i => i, i => i);
        }
    }
}