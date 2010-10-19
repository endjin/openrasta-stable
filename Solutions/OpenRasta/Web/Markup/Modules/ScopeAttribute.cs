// The Table module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_tablemodule

namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Web.Markup.Attributes.Annotations;

    public class ScopeAttribute : EnumAttributeCore
    {
        public ScopeAttribute() : base(Factory<Scope>)
        {
        }
    }
}