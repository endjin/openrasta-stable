namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    #endregion

    /// <summary>
    /// Annotation for attributes using 1 and 0 as boolean values.
    /// </summary>
    public class DigitBooleanAttribute : XhtmlAttributeCore
    {
        protected override Func<IAttribute> Factory(string propertyName)
        {
            return () => (IAttribute)new XhtmlAttributeNode<bool>(
                           propertyName,
                           true,
                           b => b ? "1" : "0", 
                           s => string.Compare(s.Trim(), "1", StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}