namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public class FrameAttribute : EnumAttributeCore
    {
        public FrameAttribute() : base(Factory<Frame>)
        {
        }
    }
}