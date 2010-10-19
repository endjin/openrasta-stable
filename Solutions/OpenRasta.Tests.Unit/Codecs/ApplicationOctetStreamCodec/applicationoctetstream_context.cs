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

namespace OpenRasta.Tests.Unit.Codecs.ApplicationOctetStreamCodec
{
    #region Using Directives

    using OpenRasta.Codecs;
    using OpenRasta.IO;
    using OpenRasta.Tests.Unit.Codecs.MediaTypeDictionary;
    using OpenRasta.Web;

    #endregion

    public class applicationoctetstream_context : media_type_reader_context<ApplicationOctetStreamCodec>
    {
        protected void given_request_entity_stream()
        {
            given_request_entity_stream(1024);
        }

        protected void given_request_entity_stream(int length)
        {
            given_request_stream(stream=>stream.Write(new byte[length]));
        }
        
        protected override ApplicationOctetStreamCodec CreateCodec(ICommunicationContext context)
        {
            return new ApplicationOctetStreamCodec();
        }
        
        protected void given_content_disposition_header(string p)
        {
            Context.Request.Entity.Headers.ContentDisposition = new ContentDispositionHeader(p);
        }
    }
}