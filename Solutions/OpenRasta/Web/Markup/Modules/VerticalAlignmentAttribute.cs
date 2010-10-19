namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public class VerticalAlignmentAttribute : EnumAttributeCore
    {
        public VerticalAlignmentAttribute() : base(Factory<VerticalAlignment>)
        {
        }
    }
}