namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    public class NMTOKENSAttribute : XhtmlAttributeCore
    {
        public NMTOKENSAttribute()
        {
        }

        public NMTOKENSAttribute(string attribName) : base(attribName)
        {
        }

        protected override Func<IAttribute> Factory(string propertyName)
        {
            return () => (IAttribute)new NMTOKENSAttributeNode(propertyName);
        }
    }
}