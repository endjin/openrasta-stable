namespace OpenRasta.Contracts.CodeDom.Compiler
{
    public interface ICodeSnippetModifier
    {
        bool CanProcessObject(object source, object value);

        string ProcessObject(object source, object value);
    }
}