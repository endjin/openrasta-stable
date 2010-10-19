namespace OpenRasta.Contracts.Web.Markup
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Contracts.Web.Markup.Attributes;

    #endregion

    public interface IElement : INode
    {
        IList<INode> ChildNodes { get; }

        IEnumerable<IElement> ChildElements { get; }

        string TagName { get; set; }

        IAttributeCollection Attributes { get; }

        IList<Type> ContentModel { get; }

        string InnerText { get; }

        string OuterXml { get; }

        bool IsVisible { get; set; }

        void Prepare();
    }
}