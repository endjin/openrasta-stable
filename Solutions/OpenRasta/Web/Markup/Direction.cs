namespace OpenRasta.Web.Markup
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public enum Direction
    {
        Ltr,
        Rtl
    }

    public class DirectionAttributeAttribute : EnumAttributeCore
    {
        public DirectionAttributeAttribute() : base(Factory<Direction>)
        {
        }
    }
}