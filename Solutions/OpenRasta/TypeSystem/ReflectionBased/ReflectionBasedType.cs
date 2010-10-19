namespace OpenRasta.TypeSystem.ReflectionBased
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using OpenRasta.Binding;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.DI;

    #endregion

    /// <summary>
    /// Represents a CLR-based type.
    /// </summary>
    [DebuggerDisplay("Name={TargetType.Name}, FullName={TargetType.ToString()}")]
    public class ReflectionBasedType : ReflectionBasedMember<ITypeBuilder>, IResolverAwareType
    {
        public ReflectionBasedType(ITypeSystem typeSystem, Type type) : base(typeSystem, type)
        {
        }

        public override IType Type
        {
            get { return this; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ReflectionBasedType))
            {
                return false;
            }

            return TargetType.Equals(((ReflectionBasedType)obj).TargetType);
        }

        public override int GetHashCode()
        {
            return TargetType.GetHashCode();
        }

        public int CompareTo(IType other)
        {
            if (other == null || other.StaticType == null)
            {
                return -1;
            }

            return TargetType.GetInheritanceDistance(other.StaticType);
        }

        public object CreateInstance(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }

            return resolver.Resolve(TargetType, UnregisteredAction.AddAsTransient);
        }

        public ITypeBuilder CreateBuilder()
        {
            return new TypeBuilder(this);
        }

        public virtual object CreateInstance()
        {
            return TargetType.CreateInstance();
        }

        public bool IsAssignableFrom(IType member)
        {
            return member != null && member.CompareTo(this) >= 0;
        }

        public bool TryCreateInstance<T>(IEnumerable<T> values, ValueConverter<T> converter, out object result)
        {
            result = null;
            try
            {
                result = TargetType.CreateInstanceFrom(values, converter);
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }

            return result != null;
        }
    }
}