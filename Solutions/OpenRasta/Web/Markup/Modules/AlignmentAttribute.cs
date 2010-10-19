namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public class AlignmentAttribute : EnumAttributeCore
    {
        public AlignmentAttribute() : base(Factory<Alignment>)
        {
        }
    }
}