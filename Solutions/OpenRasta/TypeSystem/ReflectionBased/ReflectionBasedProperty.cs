namespace OpenRasta.TypeSystem.ReflectionBased
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using OpenRasta.Binding;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    [DebuggerDisplay("{global::OpenRasta.TypeSystem.DebuggerStrings.Property(this)}")]
    public class ReflectionBasedProperty : ReflectionBasedMember<IPropertyBuilder>, IProperty
    {
        public ReflectionBasedProperty(ITypeSystem typeSystem, IMember parent, PropertyInfo property, object[] propertyParameters)
            : base(typeSystem, property.PropertyType)
        {
            this.PropertyParameters = propertyParameters ?? new object[0];
            this.Property = property;
            this.Owner = parent;
        }

        /// <summary>
        /// Defines the PropertyInfo of the property as defined on the owner type
        /// </summary>
        public PropertyInfo Property { get; private set; }

        public override string Name
        {
            get { return this.Property.Name; }
        }

        /// <summary>
        /// Defines the parameters for indexer properties
        /// </summary>
        public object[] PropertyParameters { get; private set; }

        /// <summary>
        /// Defines the type owning this property
        /// </summary>
        public IMember Owner { get; private set; }

        public override bool CanSetValue(object value)
        {
            return base.CanSetValue(value) && this.Property.CanWrite;
        }

        public bool CanWrite
        {
            get { return this.Property.CanWrite; }
        }

        public bool TrySetValue(object target, object value)
        {
            if (!this.Property.CanWrite)
            {
                return false;
            }

            this.Property.SetValue(target, value, this.PropertyParameters);

            return true;
        }

        public bool TrySetValue<T>(object target, IEnumerable<T> values, ValueConverter<T> converter)
        {
            if (!this.Property.CanWrite)
            {
                return false;
            }

            this.Property.SetValue(target, this.Property.PropertyType.CreateInstanceFrom(values, converter), this.PropertyParameters);
            
            return true;
        }

        public IPropertyBuilder CreateBuilder(IMemberBuilder parentBuilder)
        {
            return new PropertyBuilder(parentBuilder, this);
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

        public virtual object GetValue(object target)
        {
            try
            {
                return this.Property.GetValue(target, this.PropertyParameters);
            }
            catch
            {
                return null;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ReflectionBasedProperty))
            {
                return false;
            }

            var other = (ReflectionBasedProperty)obj;
            bool isEqual = this.Property.Equals(other.Property);
            
            if (!isEqual)
            {
                return false;
            }
            
            // equality depends on the owner stacks being equal and the index parameters being equal
            if ((this.PropertyParameters == null && other.PropertyParameters != null)
                || (this.PropertyParameters != null && other.PropertyParameters == null)
                || (this.PropertyParameters != null && this.PropertyParameters.Length != other.PropertyParameters.Length))
            {
                return false;
            }

            if (this.PropertyParameters != null)
            {
                for (int i = 0; i < this.PropertyParameters.Length; i++)
                {
                    if (!this.PropertyParameters[i].Equals(other.PropertyParameters[i]))
                    {
                        return false;
                    }
                }
            }

            List<IMember> thisOwners = this.GetCallStack().Skip(1).ToList();
            List<IMember> otherOwners = other.GetCallStack().Skip(1).ToList();
            
            if (thisOwners.Count != otherOwners.Count)
            {
                return false;
            }

            for (int i = 0; i < thisOwners.Count; i++)
            {
                if (!thisOwners[i].Equals(otherOwners[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = this.Property.GetHashCode();
            hashCode = this.GetCallStack().Skip(1).Aggregate(hashCode, (code, owner) => code ^ owner.GetHashCode());
            
            if (this.PropertyParameters != null)
            {
                hashCode = this.PropertyParameters.Aggregate(hashCode, (hash, param) => hash ^ param.GetHashCode());
            }

            return hashCode;
        }
    }
}