namespace OpenRasta.TypeSystem
{
    public class PathComponent
    {
        public PathComponent()
        {
            this.ParsedValue = string.Empty;
            this.Type = PathComponentType.None;
        }

        public PathComponentType Type { get; set; }

        public string ParsedValue { get; set; }
    }
}