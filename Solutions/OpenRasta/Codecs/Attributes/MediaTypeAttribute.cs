namespace OpenRasta.Codecs.Attributes
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Web;

    #endregion

    /// <summary>
    /// Defines the default media types supported by a codec.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class MediaTypeAttribute : Attribute
    {
        public MediaTypeAttribute(MediaType mediaType)
        {
            MediaType = mediaType;
        }

        public MediaTypeAttribute(MediaType mediaType, string extensions) : this(mediaType)
        {
            this.Extensions = ProcessExtensions(extensions);
        }

        public MediaTypeAttribute(string mediaType) : this(new MediaType(mediaType))
        {
        }

        public MediaTypeAttribute(string mediaType, string extensions) : this(new MediaType(mediaType), extensions)
        {
        }

        public IEnumerable<string> Extensions { get; private set; }

        public MediaType MediaType { get; private set; }

        public static IEnumerable<MediaTypeAttribute> Find(Type type)
        {
            return GetCustomAttributes(type, typeof(MediaTypeAttribute)).Cast<MediaTypeAttribute>();
        }

        private static IEnumerable<string> ProcessExtensions(string extensions)
        {
            return extensions.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}