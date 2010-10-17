namespace OpenRasta.TypeSystem.Surrogated
{
    using System;
    using System.Collections.Generic;

    public abstract class WrappedMember : IMember, IHasWrappedMember
    {
        private readonly IMember wrapped;
        private readonly Dictionary<string, IProperty> cachedProperty = new Dictionary<string, IProperty>();

        protected WrappedMember(IMember member)
        {
            this.wrapped = member;
        }

        public Type StaticType
        {
            get { return this.wrapped.StaticType; }
        }

        public virtual bool IsEnumerable
        {
            get { return this.wrapped.IsEnumerable; }
        }

        public virtual string Name
        {
            get { return this.wrapped.Name; }
        }

        public virtual IType Type
        {
            get { return this.wrapped.Type; }
        }

        public virtual string TypeName
        {
            get { return this.wrapped.TypeName; }
        }

        public virtual ITypeSystem TypeSystem
        {
            get { return this.wrapped.TypeSystem; }
            set { this.wrapped.TypeSystem = value; }
        }

        IMember IHasWrappedMember.WrappedMember
        {
            get { return this.wrapped; }
        }

        public virtual T FindAttribute<T>() where T : class
        {
            return this.wrapped.FindAttribute<T>();
        }

        public virtual IEnumerable<T> FindAttributes<T>() where T : class
        {
            return this.wrapped.FindAttributes<T>();
        }

        public virtual bool CanSetValue(object value)
        {
            return this.wrapped.CanSetValue(value);
        }

        public virtual IProperty GetIndexer(string parameter)
        {
            return this.CachedProperty(parameter, () => this.Reroot(this.wrapped.GetIndexer(parameter)));
        }

        public virtual IProperty GetProperty(string name)
        {
            return this.CachedProperty(name, () => this.Reroot(this.wrapped.GetProperty(name)));
        }

        public virtual IMethod GetMethod(string methodName)
        {
            return this.wrapped.GetMethod(methodName);
        }

        public virtual IList<IMethod> GetMethods()
        {
            return this.wrapped.GetMethods();
        }

        protected WrappedProperty Reroot(IProperty root)
        {
            if (root == null)
            {
                return null;
            }

            return new WrappedProperty(this, root);
        }

        protected IProperty CachedProperty(string parameter, Func<IProperty> propertyCreator)
        {
            IProperty output;

            if (!this.cachedProperty.TryGetValue(parameter, out output))
            {
                lock (this.cachedProperty)
                {
                    if (!this.cachedProperty.TryGetValue(parameter, out output))
                    {
                        this.cachedProperty[parameter] = output = propertyCreator();
                    }
                }
            }

            return output;
        }
    }
}