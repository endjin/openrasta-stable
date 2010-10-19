// The object module
// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_objectmodule
namespace OpenRasta.Web.Markup.Modules
{
    #region Using Directives

    using OpenRasta.Web.Markup.Attributes.Annotations;

    #endregion

    public class ParamValueTypeAttributeAttribute : PrimaryTypeAttributeCore
    {
        public ParamValueTypeAttributeAttribute()
            : base(Factory<ParamValueType>)
        {
        }
    }
}