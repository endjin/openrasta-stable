namespace OpenRasta.Binding
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.TypeSystem;

    #endregion

    public class KeyedValuesBinder : IObjectBinder
    {
        private readonly bool enumerable;
        private readonly string name;
        private readonly string typeName;

        private object cachedBuiltObject;
        private bool instanceConstructed;

        public KeyedValuesBinder(IType target) : this(target, target.Name)
        {
        }

        public KeyedValuesBinder(IType target, string name)
        {
            this.enumerable = !target.Equals<string>() && target.Type.IsEnumerable;
            this.Builder = target.CreateBuilder();
            this.name = name;
            this.typeName = target.TypeName;

            this.Prefixes = new List<string> { this.name, this.typeName };
            this.PathManager = new PathManager();
        }

        public bool IsEmpty
        {
            get { return !this.Builder.HasValue; }
        }

        public ICollection<string> Prefixes { get; private set; }

        protected ITypeBuilder Builder { get; private set; }

        protected IPathManager PathManager { get; set; }

        public virtual BindingResult BuildObject()
        {
            if (this.IsEmpty && !this.enumerable)
            {
                return BindingResult.Failure();
            }

            if (this.instanceConstructed)
            {
                return BindingResult.Success(this.cachedBuiltObject);
            }

            this.cachedBuiltObject = this.Builder.Create();
            this.instanceConstructed = true;
            
            return BindingResult.Success(this.cachedBuiltObject);
        }

        public virtual bool SetInstance(object builtInstance)
        {
            if (this.Builder.Value != null)
            {
                throw new InvalidOperationException("An instance was already set by passing a constructor key.");
            }

            this.instanceConstructed = false;

            return this.Builder.TrySetValue(builtInstance);
        }

        public bool SetProperty<TValue>(string key, IEnumerable<TValue> values, ValueConverter<TValue> converter)
        {
            this.instanceConstructed = false;
            var keyType = this.PathManager.GetPathType(this.Prefixes, key);

            bool success = keyType.Type == PathComponentType.Constructor ? SetConstructorValue(values, converter) : SetPropertyValue(key, keyType.ParsedValue, values, converter);

            if (!success)
            {
                success = SetPropertyValue(key, key, values, converter);
            }

            return success;
        }

        private bool SetConstructorValue<TValue>(IEnumerable<TValue> values, ValueConverter<TValue> converter)
        {
            return this.Builder.TrySetValue(values, converter);
        }

        private bool SetPropertyValue<TValue>(string key, string property, IEnumerable<TValue> values, ValueConverter<TValue> converter)
        {
            var propertyBuilder = this.Builder.GetProperty(property ?? key);
            
            if (propertyBuilder == null)
            {
                return false;
            }

            return propertyBuilder.TrySetValue(values, converter);
        }
    }
}