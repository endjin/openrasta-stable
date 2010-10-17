namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    using System;

    using OpenRasta.Web.Markup.Attributes.Nodes;

    public abstract class EnumAttributeCore : XhtmlAttributeCore
    {
        private readonly Func<string, Func<IAttribute>> factory;

        protected EnumAttributeCore()
        {
        }

        protected EnumAttributeCore(Func<string, Func<IAttribute>> factory) : this(null, factory)
        {
        }

        protected EnumAttributeCore(string attribName, Func<string, Func<IAttribute>> factory) : base(attribName)
        {
            this.factory = factory;
        }

        public static Func<IAttribute> Factory<T>(string propertyName)
        {
            return () => (IAttribute)new EnumAttributeNode<T>(propertyName);
        }

        protected override Func<IAttribute> Factory(string propertyName)
        {
            return this.factory(propertyName);
        }
    }
}