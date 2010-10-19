namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes; 

    #endregion

    public class URIsAttribute : XhtmlAttributeCore
    {
        public URIsAttribute()
        {
        }

        public URIsAttribute(string attribName) : base(attribName)
        {
        }

        protected override Func<IAttribute> Factory(string propertyName)
        {
            return
                () =>
                (IAttribute)
                new CharacterSeparatedAttributeNode<Uri>(propertyName, " ", u => u.ToString(), s => new Uri(s, UriKind.Absolute));
        }
    }
}