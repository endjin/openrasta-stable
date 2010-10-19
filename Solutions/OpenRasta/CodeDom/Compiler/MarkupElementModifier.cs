namespace OpenRasta.CodeDom.Compiler
{
    #region Using Directives

    using OpenRasta.Contracts.CodeDom.Compiler;
    using OpenRasta.Contracts.Web.Markup;

    #endregion

    /// <summary>
    /// Supports IElement elements, and render them as html elements dealing with their own encoding.
    /// </summary>
    public class MarkupElementModifier : ICodeSnippetModifier
    {
        public bool CanProcessObject(object source, object value)
        {
            return value is IElement;
        }

        public string ProcessObject(object source, object value)
        {
            return value.ToString();
        }
    }
}