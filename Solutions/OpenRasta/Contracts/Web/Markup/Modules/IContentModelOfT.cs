﻿namespace OpenRasta.Contracts.Web.Markup.Modules
{
    public interface IContentModel<TElement, TChild> : IContentModel
    {
        TElement this[TChild child]
        {
            get;
        }
    }
}