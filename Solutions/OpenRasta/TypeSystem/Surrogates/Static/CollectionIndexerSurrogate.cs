namespace OpenRasta.TypeSystem.Surrogates.Static
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Please do not use, it's not ready for prime time yet.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionIndexerSurrogate<T> : ISurrogate
    {
        private ICollection<T> surrogatedValue;
        private int lastSeenIndex = -1;

        object ISurrogate.Value
        {
            get { return this.surrogatedValue; }
            set { this.surrogatedValue = (ICollection<T>)value; }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                if (index == this.lastSeenIndex)
                {
                    return this.surrogatedValue.Last();
                }

                this.lastSeenIndex = index;
                this.surrogatedValue.Add(default(T));
                
                return default(T);
            }

            set
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                if (index == this.lastSeenIndex)
                {
                    var tempValues = this.surrogatedValue.ToList();
                    
                    for (int i = 0; i < tempValues.Count - 1; i++)
                    {
                        this.surrogatedValue.Add(tempValues[i]);
                    }
                }

                this.lastSeenIndex = index;
                this.surrogatedValue.Add(value);
            }
        }
    }
}