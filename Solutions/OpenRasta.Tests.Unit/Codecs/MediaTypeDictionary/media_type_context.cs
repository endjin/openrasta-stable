namespace OpenRasta.Tests.Unit.Codecs.MediaTypeDictionary
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Codecs;
    using OpenRasta.Testing.Specifications;
    using OpenRasta.Web;

    #endregion

    public class media_type_context : context
    {
        public MediaTypeDictionary<string> Repository { get; set; }

        public List<string> ThenTheResult { get; set; }

        protected override void SetUp()
        {
            base.SetUp();
            this.Repository = new MediaTypeDictionary<string>();
        }

        protected void GivenMediaType(string mediatype, string name)
        {
            this.Repository.Add(new MediaType(mediatype), name);
        }

        protected void WhenMatching(string mediaType)
        {
            this.ThenTheResult = new List<string>(this.Repository.Matching(new MediaType(mediaType)));
        }
    }
}