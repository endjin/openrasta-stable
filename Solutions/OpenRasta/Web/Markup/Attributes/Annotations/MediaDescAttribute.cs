namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    #endregion

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