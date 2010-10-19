namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    #region Using Directives

    using System;
    using System.Reflection;

    using OpenRasta.Contracts.Web.Markup.Attributes;

    #endregion

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class XhtmlAttributeCore : Attribute
    {
        protected XhtmlAttributeCore()
        {
        }

        protected XhtmlAttributeCore(string name)
        {
            this.Name = name;
        }

        public string DefaultValue { get; set; }

        public string Name { get; private set; }

        public Func<IAttribute> Factory(PropertyInfo pi)
        {
            return () =>
                {
                    var value = Factory(Name ?? pi.Name)();
                
                    if (DefaultValue != null)
                    {
                        value.DefaultValue = DefaultValue;
                    }

                    return value;
                };
        }

        protected abstract Func<IAttribute> Factory(string propertyName);
    }
}