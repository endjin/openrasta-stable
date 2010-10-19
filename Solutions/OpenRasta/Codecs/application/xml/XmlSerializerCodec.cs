namespace OpenRasta.Codecs.Application.xml
{
    #region Using Directives

    using System;
    using System.Xml.Serialization;

    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;

    #endregion

    [MediaType("application/xml;q=0.4", ".xml")]
    public class XmlSerializerCodec : XmlCodec
    {
        public override object ReadFrom(IHttpEntity request, IType destinationType, string parameterName)
        {
            if (destinationType.StaticType == null)
            {
                throw new InvalidOperationException();
            }

            return new XmlSerializer(destinationType.StaticType).Deserialize(request.Stream);
        }

        protected override void WriteToCore(object obj, IHttpEntity response)
        {
            var serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(Writer, obj);
        }
    }
}