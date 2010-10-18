namespace OpenRasta.TypeSystem.Surrogated
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using OpenRasta.Binding;

    [DebuggerDisplay("Name={OriginalNativeType.Name}, Alien={OriginalAlienType.Name}")]
    public class AlienType : AlienMember, IType
    {
        public AlienType(IType alienType, IMember nativeType) : base(alienType, nativeType)
        {
            this.OriginalAlienType = alienType;
            this.OriginalNativeType = nativeType;
        }

        public override IType Type
        {
            get { return this; }
        }

        protected IType OriginalAlienType { get; private set; }

        protected IMember OriginalNativeType { get; set; }

        public int CompareTo(IType other)
        {
            return this.OriginalAlienType.CompareTo(other);
        }

        public ITypeBuilder CreateBuilder()
        {
            throw new NotImplementedException();
        }

        public object CreateInstance()
        {
            return this.OriginalAlienType.CreateInstance();
        }

        public bool IsAssignableFrom(IType type)
        {
            return this.OriginalAlienType.IsAssignableFrom(type);
        }

        public bool TryCreateInstance<T>(IEnumerable<T> values, ValueConverter<T> converter, out object result)
        {
            return this.OriginalAlienType.TryCreateInstance(values, converter, out result);
        }
    }
}