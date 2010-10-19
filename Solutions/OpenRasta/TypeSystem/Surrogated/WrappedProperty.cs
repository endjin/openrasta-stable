namespace OpenRasta.TypeSystem.Surrogated
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using OpenRasta.Binding;
    using OpenRasta.Contracts.TypeSystem;

    [DebuggerDisplay("{global::OpenRasta.TypeSystem.DebuggerStrings.Property(_property)}")]
    public class WrappedProperty : WrappedMember, IProperty
    {
        private readonly IProperty property;

        public WrappedProperty(IMember owner, IProperty property) : base(property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.property = property;
            this.Owner = owner;
        }

        public bool CanWrite
        {
            get { return this.property.CanWrite; }
        }

        public IMember Owner { get; private set; }

        public object[] PropertyParameters
        {
            get { return this.property.PropertyParameters; }
        }

        public IPropertyBuilder CreateBuilder(IMemberBuilder builder)
        {
            return this.property.CreateBuilder(builder);
        }

        public IEnumerable<IMember> GetCallStack()
        {
            IMember current = this;
            
            while (current != null)
            {
                yield return current;
                
                var currentIsProp = current as IProperty;
                
                if (currentIsProp != null)
                {
                    current = currentIsProp.Owner;
                }
                else
                {
                    break;
                }
            }
        }

        public object GetValue(object target)
        {
            return this.property.GetValue(target);
        }

        public bool TrySetValue<T>(object target, IEnumerable<T> values, ValueConverter<T> converter)
        {
            return this.property.TrySetValue(target, values, converter);
        }

        public bool TrySetValue(object target, object value)
        {
            return this.property.TrySetValue(target, value);
        }
    }
}