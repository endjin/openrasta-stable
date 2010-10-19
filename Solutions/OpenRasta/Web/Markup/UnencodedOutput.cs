namespace OpenRasta.Web.Markup
{
    public class UnencodedOutput
    {
        public string Value { get; set; }
        
        public static explicit operator UnencodedOutput(string text)
        {
            return new UnencodedOutput { Value = text };
        }

        public static implicit operator string(UnencodedOutput output)
        {
            return output.Value;
        }
    }
}