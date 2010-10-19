namespace OpenRasta.Web.Markup.Controls
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Collections;
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Exceptions;
    using OpenRasta.Web.Markup.Elements;

    #endregion

    public class ValidationSummaryControl : Element
    {
        public ValidationSummaryControl(IList<Error> errors)
        {
            if (errors == null)
            {
                errors = new List<Error>();
            }

            var element = ul;
            var elements = errors.Select(error => li[error.Message]).ForEach(li => element.ChildNodes.Add(li));
            var currentElement = this[div.Class("error")[element]];
        }
    }
}