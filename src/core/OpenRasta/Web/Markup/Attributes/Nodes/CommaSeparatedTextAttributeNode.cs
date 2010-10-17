namespace OpenRasta.Web.Markup.Attributes.Nodes
{
    public class CommaSeparatedTextAttributeNode : CharacterSeparatedAttributeNode<string>
    {
        public CommaSeparatedTextAttributeNode(string name) : base(name, ",", i => i, i => i)
        {
            Value = new CharacterSplitterCollection(",");
        }
    }
}