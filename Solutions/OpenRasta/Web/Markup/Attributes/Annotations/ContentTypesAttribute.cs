namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    #endregion

    public class ContentTypesAttribute : XhtmlAttributeCore
    {
        public ContentTypesAttribute()
        {
        }

        public ContentTypesAttribute(string name) : base(name)
        {
        }

        protected override Func<IAttribute> Factory(string name)
        {
            return () => (IAttribute)new CharacterSeparatedAttributeNode<MediaType>(name, ",", m => m.ToString(), s => new MediaType(s));
        }
    }
}