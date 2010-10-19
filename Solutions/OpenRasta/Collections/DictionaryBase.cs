namespace OpenRasta.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class DictionaryBase<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
    {
        private readonly Dictionary<TKey, TValue> baseDictionary;

        public DictionaryBase()
        {
            this.baseDictionary = new Dictionary<TKey, TValue>();
        }

        public DictionaryBase(IEqualityComparer<TKey> comparer)
        {
            this.baseDictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public int Count
        {
            get { return this.baseDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary)this.baseDictionary).IsReadOnly; }
        }

        public ICollection<TKey> Keys
        {
            get { return this.baseDictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return this.baseDictionary.Values; }
        }

        protected IEqualityComparer<TKey> Comparer
        {
            get { return this.baseDictionary.Comparer; }
        }

        public bool IsFixedSize
        {
            get { return ((IDictionary)this.baseDictionary).IsFixedSize; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>)this.baseDictionary).IsReadOnly; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this.baseDictionary).IsSynchronized; }
        }

        ICollection IDictionary.Keys
        {
            get { return ((IDictionary)this.baseDictionary).Keys; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)this.baseDictionary).SyncRoot; }
        }

        ICollection IDictionary.Values
        {
            get { return ((IDictionary)this.baseDictionary).Values; }
        }

        public virtual TValue this[TKey key]
        {
            get { return this.baseDictionary[key]; }
            set { this.baseDictionary[key] = value; }
        }

        object IDictionary.this[object key]
        {
            get { return this[(TKey)key]; }
            set { this[(TKey)key] = (TValue)value; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.baseDictionary).CopyTo(array, index);
        }

        public virtual void Clear()
        {
            this.baseDictionary.Clear();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.baseDictionary).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.baseDictionary).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (this.baseDictionary.ContainsKey(item.Key) && (ReferenceEquals(item.Value, this.baseDictionary[item.Key]) || item.Value.Equals(this.baseDictionary[item.Key])))
            {
                return this.Remove(item.Key);
            }

            return false;
        }

        void IDictionary.Add(object key, object value)
        {
            this.Add((TKey)key, (TValue)value);
        }

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary)this.baseDictionary).Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)this.baseDictionary).GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            this.Remove((TKey)key);
        }

        public virtual void Add(TKey key, TValue value)
        {
            this.baseDictionary.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return this.baseDictionary.ContainsKey(key);
        }

        public virtual bool Remove(TKey key)
        {
            return this.baseDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.baseDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.baseDictionary).GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)this.baseDictionary).GetEnumerator();
        }
    }
}