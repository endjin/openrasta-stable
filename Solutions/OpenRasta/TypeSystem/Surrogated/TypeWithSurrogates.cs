namespace OpenRasta.TypeSystem.Surrogated
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Diagnostics;

    using OpenRasta.Binding;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    [DebuggerDisplay("Name={_wrappedType.Name}, FullName={_wrappedType.TargetType.ToString()}")]
    public class TypeWithSurrogates : MemberWithSurrogates, IType
    {
        private readonly IType wrappedType;

        public TypeWithSurrogates(IType wrappedType, IEnumerable<IType> alienTypes)
            : base(wrappedType, alienTypes)
        {
            this.wrappedType = wrappedType;
        }

        public override IType Type
        {
            get { return this; }
        }

        public int CompareTo(IType other)
        {
            if (other is TypeWithSurrogates)
            {
                return this.wrappedType.CompareTo(((TypeWithSurrogates)other).wrappedType);
            }

            return this.wrappedType.CompareTo(other);
        }

        public ITypeBuilder CreateBuilder()
        {
            return new TypeWithSurrogatesBuilder(this, AlienTypes);
        }

        public object CreateInstance()
        {
            return this.wrappedType.CreateInstance();
        }

        public bool IsAssignableFrom(IType type)
        {
            return this.wrappedType.IsAssignableFrom(type);
        }

        public bool TryCreateInstance<T>(IEnumerable<T> values, ValueConverter<T> converter, out object result)
        {
            return this.wrappedType.TryCreateInstance(values, converter, out result);
        }
    }
}