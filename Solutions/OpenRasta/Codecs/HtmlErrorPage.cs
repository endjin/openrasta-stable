namespace OpenRasta.Codecs
{
    using System.Collections.Generic;

    using OpenRasta.Web.Markup;
    using OpenRasta.Web.Markup.Elements;
    using OpenRasta.Web.Markup.Modules;

    public class HtmlErrorPage : Element
    {
        public Element Root { get; set; }

        public HtmlErrorPage(IEnumerable<Error> errors)
        {
            var exceptionBlock = this.GetExceptionBlock(errors);
            Root = this
                [html
                     [head[title["OpenRasta encountered an error."]]]
                     [body
                          [div.Class("errorList")[exceptionBlock]]
                     ]
                ];
        }

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