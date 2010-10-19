namespace OpenRasta.TypeSystem
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Binding;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public class PropertyBuilder : MemberBuilder, IPropertyBuilder
    {
        private object cachedValue;

        public PropertyBuilder(IMemberBuilder parent, IProperty property)
            : base(parent, property)
        {
            this.Owner = parent;
        }

        public int IndexAtCreation { get; set; }

        public IProperty Property
        {
            get { return Member as IProperty; }
        }

        public override bool CanWrite
        {
            get { return this.Property.CanWrite; }
        }

        public IMemberBuilder Owner { get; private set; }

        public override object Value
        {
            get { return this.cachedValue; }
        }

        public override bool HasValue
        {
            get { return this.cachedValue != null; }
        }

        public override bool TrySetValue(object value)
        {
            if (!this.Property.CanSetValue(value))
            {
                return false;
            }

            this.cachedValue = value;
            
            return true;
        }

        /// <summary>
        /// Tries to assign a property value and return <c>true</c> if it was successfully assigned or if the parent wasn't available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public override bool TrySetValue<T>(IEnumerable<T> values, ValueConverter<T> converter)
        {
            if (this.Property.Type.IsEnumerable && this.cachedValue != null)
            {
                var addMethod = this.Property.Type.GetMethod("Add");
                
                if (addMethod != null)
                {
                    var parameter = addMethod.InputMembers.FirstOrDefault();
                    var builder = parameter.Type.CreateBuilder();
                    
                    if (builder.TrySetValue(values, converter))
                    {
                        addMethod.Invoke(this.cachedValue, builder.Value);
                        
                        return true;
                    }
                }
            }

            object newValue;
            
            var success = this.Property.Type.TryCreateInstance(values, converter, out newValue);
            
            if (success)
            {
                this.cachedValue = newValue;
            }

            return success;
        }

        public override object Apply(object target, out object assignedValue)
        {
            if (target == null)
            {
                target = this.Owner.Member.Type.CreateInstance();
            }

            if (this.cachedValue != null)
            {
                this.Property.TrySetValue(target, assignedValue = this.cachedValue);
            }
            else
            {
                assignedValue = this.Property.GetValue(target);
            }

            return target;
        }
    }
}