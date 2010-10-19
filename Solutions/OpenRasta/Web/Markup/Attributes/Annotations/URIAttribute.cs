namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    #endregion

    public class URIAttribute : XhtmlAttributeCore
    {
        private readonly bool renderOnDefault;

        public URIAttribute()
        {
        }
        
        public URIAttribute(string name, bool renderOnDefault) : base(name)
        {
            this.renderOnDefault = renderOnDefault;
        }

        protected override Func<IAttribute> Factory(string propertyName)
        {
            return () => (IAttribute)new XhtmlAttributeNode<Uri>(propertyName, this.renderOnDefault, u => u.ToString(), s => new Uri(s, UriKind.RelativeOrAbsolute));
        }
    }
}