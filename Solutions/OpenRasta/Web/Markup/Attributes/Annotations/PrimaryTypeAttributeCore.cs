namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    public abstract class PrimaryTypeAttributeCore : XhtmlAttributeCore
    {
        private readonly Func<string, Func<IAttribute>> factory;

        protected PrimaryTypeAttributeCore()
        {
        }

        protected PrimaryTypeAttributeCore(Func<string, Func<IAttribute>> factory) : this(null, factory)
        {
        }

        protected PrimaryTypeAttributeCore(string attribName, Func<string, Func<IAttribute>> factory) : base(attribName)
        {
            this.factory = factory;
        }

        public static Func<IAttribute> Factory<T>(string propertyName)
        {
            return () => (IAttribute)new PrimaryTypeAttributeNode<T>(propertyName);
        }

        protected override Func<IAttribute> Factory(string propertyName)
        {
            return this.factory(propertyName);
        }
    }
}