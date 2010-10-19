namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public class RulesAttribute : EnumAttributeCore
    {
        public RulesAttribute() : base(Factory<Rules>)
        {
        }
    }
}