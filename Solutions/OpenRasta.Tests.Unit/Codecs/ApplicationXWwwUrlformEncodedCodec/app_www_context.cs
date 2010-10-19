﻿#region License

//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

#endregion

namespace OpenRasta.Tests.Unit.Codecs.ApplicationXWwwUrlformEncodedCodec
{
    #region Using Directives

    using OpenRasta.Binding;
    using OpenRasta.Codecs;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Tests.Unit.Codecs.MediaTypeDictionary;
    using OpenRasta.Web;

    #endregion

    public class app_www_context : media_type_reader_context<ApplicationXWwwFormUrlencodedObjectCodec>
    {
        protected override ApplicationXWwwFormUrlencodedObjectCodec CreateCodec(ICommunicationContext context)
        {
            return new ApplicationXWwwFormUrlencodedObjectCodec(context, new DefaultObjectBinderLocator());
        }
    }
}