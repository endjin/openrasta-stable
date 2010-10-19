namespace OpenRasta.TypeSystem
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Binding;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public class TypeBuilder : MemberBuilder, ITypeBuilder
    {
        private object cacheValue;

        public TypeBuilder(IType type) : base(null, type)
        {
            this.Changes = new PropertyDictionary(this);
        }

        public IDictionary<string, IPropertyBuilder> Changes { get; private set; }

        public override bool HasValue
        {
            get { return this.cacheValue != null || this.Changes.Count > 0; }
        }

        public IType Type
        {
            get { return Member as IType; }
        }

        public override object Value
        {
            get { return this.cacheValue; }
        }

        public override object Apply(object target, out object assignedValue)
        {
            assignedValue = this.cacheValue ?? target;
            
            if (this.cacheValue != null)
            {
                return this.cacheValue;
            }

            return target;
        }

        public override bool TrySetValue(object value)
        {
            if (this.Type.CanSetValue(value))
            {
                this.cacheValue = value;
                
                return true;
            }

            return false;
        }

        public override bool TrySetValue<T>(IEnumerable<T> values, ValueConverter<T> converter)
        {
            object result;

            if (!this.Type.TryCreateInstance(values, converter, out result))
            {
                return false;
            }

            this.cacheValue = result;
            
            return true;
        }

        /// <exception cref="ArgumentNullException"><c>instance</c> is null.</exception>
        public object Create()
        {
            return this.AssignFrame(this.cacheValue ?? this.Type.CreateInstance(), this.Assignment);
        }

        /// <exception cref="ArgumentNullException"><c>instance</c> is null.</exception>
        public object Update(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return this.AssignFrame(instance, this.Assignment);
        }

        private object AssignFrame(object instance, AssignmentFrame currentFrame)
        {
            object assignedValue;
            var hasChildren = currentFrame.Children.Any();
            
            instance = currentFrame.Builder.Apply(instance, out assignedValue);

            object newValue = assignedValue;
            
            foreach (var childFrame in currentFrame.Children.Values)
            {
                newValue = this.AssignFrame(newValue, childFrame);
            }

            if (hasChildren && currentFrame.Builder.Value != newValue)
            {
                var oldValue = currentFrame.Builder.Value;
                currentFrame.Builder.TrySetValue(newValue);
                instance = currentFrame.Builder.Apply(instance, out assignedValue);
                currentFrame.Builder.TrySetValue(oldValue);
            }

            return instance;
        }

        private class PropertyDictionary : IDictionary<string, IPropertyBuilder>
        {
            public PropertyDictionary(TypeBuilder owner)
            {
                this.Owner = owner;
            }

            public int Count
            {
                get { return this.TheOnesWithValues().Count(); }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public ICollection<string> Keys
            {
                get { return this.TheOnesWithValues().Select(kv => kv.Key).ToList(); }
            }

            public TypeBuilder Owner { get; private set; }

            public ICollection<IPropertyBuilder> Values
            {
                get { return this.TheOnesWithValues().Select(kv => kv.Value).ToList(); }
            }

            public IPropertyBuilder this[string key]
            {
                get
                {
                    IPropertyBuilder property;

                    if (!this.TryGetValue(key, out property))
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    
                    return property;
                }

                set
                {
                    throw new NotSupportedException();
                }
            }

            public void Add(KeyValuePair<string, IPropertyBuilder> item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(KeyValuePair<string, IPropertyBuilder> item)
            {
                return this.ContainsKey(item.Key);
            }

            public void CopyTo(KeyValuePair<string, IPropertyBuilder>[] array, int arrayIndex)
            {
                throw new NotSupportedException();
            }

            public bool Remove(KeyValuePair<string, IPropertyBuilder> item)
            {
                throw new NotSupportedException();
            }

            public void Add(string key, IPropertyBuilder value)
            {
                throw new NotSupportedException();
            }

            public bool ContainsKey(string key)
            {
                return this.Owner.PropertiesCache.ContainsKey(key) && this.Owner.PropertiesCache[key] != null &&
                       this.Owner.PropertiesCache[key].HasValue;
            }

            public bool Remove(string key)
            {
                throw new NotSupportedException();
            }

            public bool TryGetValue(string key, out IPropertyBuilder value)
            {
                if (this.ContainsKey(key))
                {
                    value = this.Owner.PropertiesCache[key];
                    
                    return true;
                }

                value = null;
                
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public IEnumerator<KeyValuePair<string, IPropertyBuilder>> GetEnumerator()
            {
                foreach (var value in this.TheOnesWithValues())
                {
                    yield return value;
                }
            }

            private IEnumerable<KeyValuePair<string, IPropertyBuilder>> TheOnesWithValues()
            {
                return this.Owner.PropertiesCache.Where(kv => kv.Value != null && kv.Value.HasValue);
            }
        }
    }
}