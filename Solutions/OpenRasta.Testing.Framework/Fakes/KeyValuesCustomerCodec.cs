namespace OpenRasta.Testing.Framework.Fakes
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.Binding;
    using OpenRasta.Codecs;
    using OpenRasta.Collections.Specialized;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;
    using OpenRasta.TypeSystem;
    using OpenRasta.Web;

    public class KeyValuesCustomerCodec : Codec, IKeyedValuesMediaTypeReader<string>, IMediaTypeReader
    {
        
        public IEnumerable<KeyedValues<string>> ReadKeyValues(IHttpEntity entity) {
            yield return new KeyedValues<string>("FirstName", new[] { "John" }, Converter);
            yield return new KeyedValues<string>("LastName", new[] { "Doe" }, Converter);
            yield return new KeyedValues<string>("Username", new[] { "johndoe" }, Converter);
        }

        static BindingResult Converter(string t1, Type t2) { return BindingResult.Success(t1); }
        public object ReadFrom(IHttpEntity request, IType destinationType, string destinationName)
        {
            return new Customer {FirstName = "Jean", LastName = "Dupont", Username = "jeandupont"};
        }
    }
}