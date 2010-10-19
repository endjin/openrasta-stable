namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    public class ContentTypeAttribute : XhtmlAttributeCore
    {
        public ContentTypeAttribute()
        {
        }

        public ContentTypeAttribute(string attribName) : base(attribName)
        {
        }

        protected override Func<IAttribute> Factory(string propertyName)
        {
            return () => (IAttribute)new XhtmlAttributeNode<MediaType>(propertyName, false, m => m.ToString(), s => new MediaType(s));
        }
    }
}