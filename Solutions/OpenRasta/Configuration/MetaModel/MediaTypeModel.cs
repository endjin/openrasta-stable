namespace OpenRasta.Configuration.MetaModel
{
    using System.Collections.Generic;

    using OpenRasta.Web;

    public class MediaTypeModel
    {
        public MediaTypeModel()
        {
            this.Extensions = new List<string>();
        }

        public ICollection<string> Extensions { get; set; }

        public MediaType MediaType { get; set; }
    }
}