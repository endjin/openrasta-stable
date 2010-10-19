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
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;
    using OpenRasta.TypeSystem;
    using OpenRasta.Web;

    #endregion

    [MediaType("multipart/form-data;q=0.5")]
    [SupportedType(typeof(IEnumerable<IMultipartHttpEntity>))]
    [SupportedType(typeof(IDictionary<string, IList<IMultipartHttpEntity>>))]
    public class MultipartFormDataObjectCodec : AbstractMultipartFormDataCodec, IMediaTypeReader
    {
        public MultipartFormDataObjectCodec(ICommunicationContext context, ICodecRepository codecs, IDependencyResolver container, ITypeSystem typeSystem, IObjectBinderLocator binderLocator)
            : base(context, codecs, container, typeSystem, binderLocator)
        {
        }

        public object ReadFrom(IHttpEntity request, IType destinationType, string parameterName)
        {
            if (destinationType.IsAssignableFrom<IEnumerable<IMultipartHttpEntity>>())
            {
                var multipartReader = new MultipartReader(request.ContentType.Boundary, request.Stream);
                return multipartReader.GetParts();
            }

            if (destinationType.IsAssignableFrom<IDictionary<string, IList<IMultipartHttpEntity>>>())
            {
                return FormData(request);
            }

            var binder = BinderLocator.GetBinder(destinationType);
            
            if (binder == null)
            {
                throw new InvalidOperationException("Cannot find a binder to create the object");
            }

            binder.Prefixes.Add(parameterName);
            bool wasAnyKeyUsed = ReadKeyValues(request).Aggregate(false, (wasUsed, kv) => kv.SetProperty(binder) || wasUsed);
            var result = binder.BuildObject();

            return wasAnyKeyUsed && result.Successful ? result.Instance : Missing.Value;
        }
    }
}