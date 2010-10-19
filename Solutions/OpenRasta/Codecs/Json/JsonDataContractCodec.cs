namespace OpenRasta.Codecs.Json
{
    #region Using Directives

    using System;
    using System.Runtime.Serialization.Json;

    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;

    #endregion

    [MediaType("application/json;q=0.5", "json")]
    public class JsonDataContractCodec : IMediaTypeReader, IMediaTypeWriter
    {
        public object Configuration { get; set; }

        public object ReadFrom(IHttpEntity request, IType destinationType, string paramName)
        {
            if (destinationType.StaticType == null)
            {
                throw new InvalidOperationException();
            }
            
            return new DataContractJsonSerializer(destinationType.StaticType).ReadObject(request.Stream);
        }

        public void WriteTo(object entity, IHttpEntity response, string[] paramneters)
        {
            if (entity == null)
            {
                return;
            }

            var serializer = new DataContractJsonSerializer(entity.GetType());
            serializer.WriteObject(response.Stream, entity);
        }
    }
}