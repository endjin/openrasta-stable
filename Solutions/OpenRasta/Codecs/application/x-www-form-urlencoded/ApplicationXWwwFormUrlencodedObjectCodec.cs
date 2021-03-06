namespace OpenRasta.Codecs
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;

    #endregion

    [MediaType("application/x-www-form-urlencoded;q=0.5")]
    [SupportedType(typeof(IDictionary<string, string[]>))]
    [SupportedType(typeof(Dictionary<string, string[]>))]
    public class ApplicationXWwwFormUrlencodedObjectCodec : AbstractApplicationXWwwFormUrlencodedCodec, IMediaTypeReader
    {
        public ApplicationXWwwFormUrlencodedObjectCodec(ICommunicationContext context, IObjectBinderLocator locator) : base(context, locator)
        {
        }

        public object ReadFrom(IHttpEntity request, IType destinationType, string destinationName)
        {
            if (IsRawDictionary(destinationType))
            {
                return FormData(request);
            }

            var binder = this.BinderLocator.GetBinder(destinationType);
            
            if (binder == null)
            {
                throw new InvalidOperationException("Cannot find a binder to create the object");
            }

            binder.Prefixes.Add(destinationName);
            bool wasAnyKeyUsed = ReadKeyValues(request).Aggregate(false, (wasUsed, kv) => kv.SetProperty(binder) || wasUsed);
            var result = binder.BuildObject();

            return wasAnyKeyUsed && result.Successful ? result.Instance : Missing.Value;
        }
    }
}