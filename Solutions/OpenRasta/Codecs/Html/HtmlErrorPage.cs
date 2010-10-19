namespace OpenRasta.Codecs.Html
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Exceptions;
    using OpenRasta.Web.Markup.Elements;
    using OpenRasta.Web.Markup.Modules;

    #endregion

    public class HtmlErrorPage : Element
    {
        public HtmlErrorPage(IEnumerable<Error> errors)
        {
            var exceptionBlock = this.GetExceptionBlock(errors);
            Root = this[html
                [head
                    [title["OpenRasta encountered an error."]]]
                [body
                    [div.Class("errorList")[exceptionBlock]]]];
        }

        public Element Root { get; set; }

        private IDlElement GetExceptionBlock(IEnumerable<Error> errors)
        {
            var exceptionBlock = dl;

            foreach (var error in errors)
            {
                exceptionBlock = exceptionBlock
                    [dt[error.Title]]
                    [dd[pre[error.Message]]];
            }

            return exceptionBlock;
        }
    }
}