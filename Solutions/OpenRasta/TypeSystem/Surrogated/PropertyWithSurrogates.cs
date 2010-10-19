namespace OpenRasta.TypeSystem.Surrogated
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Diagnostics;

    using OpenRasta.Binding;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    [DebuggerDisplay("{global::OpenRasta.TypeSystem.DebuggerStrings.Property(this)}")]
    public class PropertyWithSurrogates : MemberWithSurrogates, IProperty
    {
        private readonly IProperty nativeProperty;

        public PropertyWithSurrogates(IProperty nativeProperty, IEnumerable<IType> alienTypes) : base(nativeProperty, alienTypes)
        {
            this.nativeProperty = nativeProperty;
        }

        public bool CanWrite
        {
            get { return this.nativeProperty.CanWrite; }
        }

        public IMember Owner
        {
            get { return this.nativeProperty.Owner; }
        }

        public object[] PropertyParameters
        {
            get { return this.nativeProperty.PropertyParameters; }
        }

        public IPropertyBuilder CreateBuilder(IMemberBuilder parentBuilder)
        {
            return new PropertyWithSurrogatesBuilder(this, parentBuilder, AlienTypes);
        }

        public IEnumerable<IMember> GetCallStack()
        {
            return this.nativeProperty.GetCallStack();
        }

        public object GetValue(object target)
        {
            return this.nativeProperty.GetValue(target);
        }

        public bool TrySetValue<T>(object target, IEnumerable<T> values, ValueConverter<T> converter)
        {
            return this.nativeProperty.TrySetValue(target, values, converter);
        }

        public bool TrySetValue(object target, object value)
        {
            return this.nativeProperty.TrySetValue(target, value);
        }
    }
}