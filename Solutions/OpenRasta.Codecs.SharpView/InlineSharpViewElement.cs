using System;
using System.Linq.Expressions;
using OpenRasta.Web.Markup;

namespace OpenRasta.Codecs.SharpView
{
    using OpenRasta.Contracts.Web.Markup;

    public class InlineSharpViewElement : SharpViewElement
    {
        public InlineSharpViewElement(Expression<Func<IElement>> elementRoot)
        {
            Root = elementRoot;
        }
    }
}