namespace OpenRasta.Codecs
{
    using OpenRasta.Web;
    using OpenRasta.Web.Markup.Elements;

    public class OperationResultPage : Element
    {
        public OperationResultPage(OperationResult result)
        {
            this.Root = this[this.html[this.head[this.title[result.Title]]][this.body[h1[result.Title]][p[result.Description]]]];
        }

        protected Element Root { get; set; }
    }
}
