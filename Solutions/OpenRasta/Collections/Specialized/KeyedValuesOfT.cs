namespace OpenRasta.Collections.Specialized
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Binding;
    using OpenRasta.Contracts.Binding;

    #endregion

    /// <summary>
    /// Represent a key associated with a series of typed values and a converter, used to match key and values pairs with binders.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    public class KeyedValues<T> : KeyedValues
    {
        public KeyedValues(string key, IEnumerable<T> values, ValueConverter<T> converter)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            Key = key;
            this.Values = values;
            this.Converter = converter;
        }

        public ValueConverter<T> Converter { get; private set; }

        public IEnumerable<T> Values { get; private set; }

        public override bool SetProperty(IObjectBinder binder)
        {
            return this.WasUsed = binder.SetProperty(this.Key, this.Values, this.Converter);
        }
    }
}