// http://www.w3.org/TR/xhtml-modularization/abstract_modules.html#s_listmodule

namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Modules;

    /// <summary>
    /// Represents the &lt;dl&gt; element
    /// </summary>
    public interface IDlElement : IAttributesCommon,
                                  IContentSetList,
                                  IContentModel<IDlElement, IDtElement>,
                                  IContentModel<IDlElement, IDdElement>
    {
    }
}