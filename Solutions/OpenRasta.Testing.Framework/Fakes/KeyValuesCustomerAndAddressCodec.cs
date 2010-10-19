﻿namespace OpenRasta.Testing.Framework.Fakes
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

    internal class KeyValuesCustomerAndAddressCodec : Codec, IKeyedValuesMediaTypeReader<string>, IMediaTypeReader
    {

        public IEnumerable<KeyedValues<string>> ReadKeyValues(IHttpEntity entity)
        {
            yield return new KeyedValues<string>("Customer.FirstName", new[] { "John" }, Converter);
            yield return new KeyedValues<string>("Customer.LastName", new[] { "Doe" }, Converter);
            yield return new KeyedValues<string>("Customer.Username", new[] { "johndoe" }, Converter);
            yield return new KeyedValues<string>("Address.City", new[] { "London" }, Converter);
        }

        static BindingResult Converter(string t1, Type t2) { return BindingResult.Success(t1); }

        public object ReadFrom(IHttpEntity request, IType destinationType, string destinationName)
        {
            return null;
        }
    }
}