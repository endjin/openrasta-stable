namespace OpenRasta.Web.Markup
{
    public class r : UnencodedOutput
    {
        public static explicit operator r(string text)
        {
            return new r { Value = text };
        }

        public static implicit operator string(r val)
        {
            return val.Value;
        }
    }
}