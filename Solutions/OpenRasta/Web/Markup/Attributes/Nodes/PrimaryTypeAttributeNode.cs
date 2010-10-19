namespace OpenRasta.Web.Markup.Attributes.Nodes
{
    using System;

    public class PrimaryTypeAttributeNode<T> : XhtmlAttributeNode<T>
    {
        public PrimaryTypeAttributeNode(string name) : this(name, false)
        {
        }

        public PrimaryTypeAttributeNode(string name, bool renderWhenDefault) : base(name, renderWhenDefault, Write, Read)
        {
        }

        public PrimaryTypeAttributeNode(string name, T defaultValue) : this(name, defaultValue, false)
        {
        }

        public PrimaryTypeAttributeNode(string name, T defaultValue, bool renderWhenDefault) : base(name, renderWhenDefault, Write, Read)
        {
            DefaultValue = Write(defaultValue);
        }

        private static string Write(T value)
        {
            return (string)Convert.ChangeType(value, typeof(string));
        }

        private static T Read(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}