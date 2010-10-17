namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    using System;

    using OpenRasta.Web.Markup.Attributes.Nodes;

    public class MediaDescAttribute : XhtmlAttributeCore
    {
        public MediaDescAttribute()
        {
        }

        public MediaDescAttribute(string attribName) : base(attribName)
        {
        }

        protected override Func<IAttribute> Factory(string propertyName)
        {
            return () => (IAttribute)new CommaSeparatedTextAttributeNode(propertyName);
        }
    }
}