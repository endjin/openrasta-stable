namespace OpenRasta.Tests.Unit.Codecs.MultipartFormDataCodec
{
    #region Using Directives

    using System.Text;

    using OpenRasta.IO;
    using OpenRasta.Web;

    #endregion

    public static class MultipartExtensions
    {
        public static MultipartHttpEntity WithEntity(this MultipartHttpEntity entity, string content)
        {
            entity.Stream.Position = 0;
            entity.Stream.Write(Encoding.Default.GetBytes(content));
            entity.Stream.Position = 0;

            return entity;
        }
    }
}