namespace OpenRasta.Contracts.Codecs
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Collections.Specialized;
    using OpenRasta.Contracts.Web;

    #endregion

    /// <summary>
    /// Represents a codec that can transform an entity into a set of named values.
    /// </summary>
    /// <typeparam name="TValue">The type of the values associated with each key.</typeparam>
    public interface IKeyedValuesMediaTypeReader<TValue> : ICodec
    {
        IEnumerable<KeyedValues<TValue>> ReadKeyValues(IHttpEntity entity);
    }
}