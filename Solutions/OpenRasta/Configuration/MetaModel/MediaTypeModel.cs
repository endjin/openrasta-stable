namespace OpenRasta.Configuration.MetaModel
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Web;

    #endregion

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