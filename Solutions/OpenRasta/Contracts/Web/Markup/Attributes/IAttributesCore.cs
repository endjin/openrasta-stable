namespace OpenRasta.Contracts.Web.Markup.Attributes
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Xml;

    using OpenRasta.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Annotations;

    #endregion

    public interface IAttributesCore : IIDAttribute, ITitleAttribute
    {
        [NMTOKENS]
        IList<string> Class { get; }
        
        [XmlSpaceAttribute]
        XmlSpace XmlSpace { get; set; }
    }
}