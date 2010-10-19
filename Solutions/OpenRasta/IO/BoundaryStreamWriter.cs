namespace OpenRasta.IO
{
    #region Using Directives

    using System.IO;
    using System.Text;

    using OpenRasta.Contracts.Web;

    #endregion

    public class BoundaryStreamWriter
    {
        public BoundaryStreamWriter(string boundary, Stream baseStream, Encoding streamEncoding)
        {
        }

        public void Write(IHttpEntity entity)
        {
        }
    }
}