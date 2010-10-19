namespace OpenRasta.CodeDom.Compiler
{
    #region Using Directives

    using OpenRasta.Contracts.CodeDom.Compiler;
    using OpenRasta.Web.Markup;

    #endregion

    /// <summary>
    /// Supports writing data without conversion to the output stream, whenever they are of type UnencodedOutput.
    /// </summary>
    public class UnencodedOutputModifier : ICodeSnippetModifier
    {
        public bool CanProcessObject(object source, object value)
        {
            return value is UnencodedOutput;
        }

        public string ProcessObject(object source, object value)
        {
            return ((UnencodedOutput)value).Value;
        }
    }
}