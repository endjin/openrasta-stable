// Defines the Forms module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_extformsmodule

namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using OpenRasta.Web.Markup.Attributes.Annotations;

    #endregion

    public class InputTypeAttribute:EnumAttributeCore
    {
        public InputTypeAttribute():base(Factory<InputType>){}
    }
}