namespace OpenRasta.Web.Markup.Attributes
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public class ButtonTypeAttributeAttribute : PrimaryTypeAttributeCore
    {
        public ButtonTypeAttributeAttribute() : base(Factory<ButtonType>)
        {
        }
    }
}