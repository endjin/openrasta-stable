namespace OpenRasta.Web.Markup.Controls
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Exceptions;
    using OpenRasta.Web.Markup.Elements;

    #endregion

    public static class ValidationSummaryExtensions
    {
        public static Element ValidationErrors(this IXhtmlAnchor anchor, IList<Error> errors)
        {
            if (errors == null || errors.Count == 0)
            {
                return null;
            }

            return new ValidationSummaryControl(errors);
        }
    }
}