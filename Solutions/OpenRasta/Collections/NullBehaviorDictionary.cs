namespace OpenRasta.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides an implementation of IDictionary&lt;TKey,TValue&gt; that automatically replaces missing values
    /// with null.
    /// </summary>
    public class NullBehaviorDictionary<TKey, TValue> : DictionaryBase<TKey, TValue>
    {
        public NullBehaviorDictionary()
        {
        }

        public NullBehaviorDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        public override TValue this[TKey key]
        {
            get { return ContainsKey(key) ? base[key] : default(TValue); }
            set { base[key] = value; }
        }
    }
}