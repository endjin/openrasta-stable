namespace OpenRasta.Web.Markup.Attributes
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.Web.Markup.Attributes.Annotations;

    public interface IAcceptAttribute
    {
        [ContentTypes]
        IList<MediaType> Accept { get; }
    }

    public interface IAccessKeyAttribute
    {
        [Character]
        char? AccessKey { get; set; }
    }

    public interface IAltAttribute
    {
        [Text]
        string Alt { get; set; }
    }

    public interface ITabIndexAttribute
    {
        [Number]
        int? TabIndex { get; set; }
    }

    public interface IDisabledAttribute
    {
        [Boolean]
        bool Disabled { get; set; }
    }

    public interface IReadOnlyAttribute
    {
        [Boolean]
        bool ReadOnly { get; set; }
    }

    public interface ISrcAttribute
    {
        [URI]
        Uri Src { get; set; }
    }

    public interface INameAttribute 
    {
        [CDATA]
        string Name { get; set; }
    }

    public interface ISizeAttribute
    {
        [Number]
        int? Size { get; set; }
    }

    public interface IValueAttribute 
    {
        [CDATA]
        string Value { get; set; }
    }

    public interface ILabelAttribute 
    {
        [CDATA]
        string Label { get; set; }
    }

    public interface IIDAttribute 
    {
        [ID]
        string ID { get; set; }
    }

    public interface ITypeAttribute
    {
        [ContentType]
        MediaType Type { get; set; }
    }

    public interface ICharSetAttribute
    {
        [Charset]
        string CharSet { get; set; }
    }

    public interface ITitleAttribute
    {
        [CDATA]
        string Title { get; set; }
    }

    public interface ILinkRelationshipAttribute
    {
        [LinkTypes]
        IList<string> Rel { get; set; }
        [LinkTypes]
        IList<string> Rev { get; set; }
    }

    public interface IHrefAttribute
    {
        [URI]
        Uri Href { get; set; }
        [LanguageCode]
        string HrefLang { get; set; }
    }

    public interface IMediaAttribute
    {
        [MediaDesc]
        IList<string> Media { get; }
    }

    public interface ILongDescAttribute
    {
        [URI]
        Uri LongDesc { get; set; }
    }

    public interface IWidthAttribute
    {
        [Length]
        string Width { get; set; }
    }

    public interface IWidthHeightAttribute : IWidthAttribute
    {
        [Length]
        string Height { get; set; }
    }

    public interface ICiteAttribute
    {
        [URI]
        Uri Cite { get; set; }
    }
}