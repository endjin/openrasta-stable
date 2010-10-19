// ReSharper disable UnusedMember.Global
namespace OpenRasta.TypeSystem.Surrogates
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using OpenRasta.Contracts.TypeSystem.Surrogates;
    using OpenRasta.TypeSystem.ReflectionBased;

    #endregion

    /// <summary>
    /// Provides a surrogate for types implementing <see cref="IList" />(Of T)
    /// </summary>
    public class ListIndexerSurrogate<T> : ISurrogate
    {
        private readonly Dictionary<int, int> binderIndexToRealIndex = new Dictionary<int, int>();
        
        private IList<T> value;

        public object Value
        {
            get
            {
                return this.value;
            }

            set
            {
                if (value.GetType().InheritsFrom(typeof(ListIndexerSurrogate<>)))
                {
                    this.value = new List<T>();
                }
                else if (value is IList<T>)
                {
                    this.value = (IList<T>)value;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public T this[int index]
        {
            get
            {
                if (this.binderIndexToRealIndex.ContainsKey(index))
                {
                    int realIndex = this.binderIndexToRealIndex[index];
                    
                    return this.value[realIndex];
                }

                return default(T);
            }

            set
            {
                if (this.binderIndexToRealIndex.ContainsKey(index))
                {
                    this.value[this.binderIndexToRealIndex[index]] = value;
                }
                else
                {
                    this.value.Add(value);
                    int realIndex = this.value.IndexOf(value);
                    this.binderIndexToRealIndex[index] = realIndex;
                }
            }
        }
    }
}

// ReSharper restore UnusedMember.Global