namespace OpenRasta.Codecs.Xml
{
    #region Using Directives

    using System;
    using System.Runtime.Serialization;

    using OpenRasta.Codecs.Application.xml;
    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;

    #endregion

    [MediaType("application/xml;q=0.5", "xml")]
    public class XmlDataContractCodec : XmlCodec
    {
        public override object ReadFrom(IHttpEntity request, IType destinationType, string parameterName)
        {
            if (destinationType.StaticType == null)
            {
                throw new InvalidOperationException();
            }

            return new DataContractSerializer(destinationType.StaticType).ReadObject(request.Stream);
        }

        protected override void WriteToCore(object entity, IHttpEntity response)
        {
            new DataContractSerializer(entity.GetType()).WriteObject(Writer, entity);
        }
    }
}