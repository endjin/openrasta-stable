namespace OpenRasta.Web.Markup.Attributes.Nodes
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    #endregion

    public class CharacterSeparatedAttributeNode<T> : XhtmlAttributeNode<IList<T>>
    {
        private readonly Func<T, string> write;

        private readonly Func<string, T> read;

        public CharacterSeparatedAttributeNode(string name, string separator, Func<T, string> write, Func<string, T> read) : base(name, false)
        {
            this.write = write;
            this.read = read;
            this.Writer = this.Write;
            this.Reader = this.Read;
            Value = new List<T>();
            this.SeparatorCharacter = separator;
        }

        public override bool IsDefault
        {
            get
            {
                return Value.Count == 0;
            }
        }

        public string SeparatorCharacter { get; set; }

        private string[] Split(string value)
        {
            return value.Split(new[] { this.SeparatorCharacter }, StringSplitOptions.RemoveEmptyEntries);
        }

        private IList<T> Read(string value)
        {
            string[] entries = this.Split(value);
            
            Value.Clear();

            foreach (var entry in entries)
            {
                Value.Add(this.read(entry));
            }

            return Value;
        }

        private string Write(IList<T> values)
        {
            var sb = new StringBuilder();

            foreach (var val in values)
            {
                if (sb.Length > 0)
                {
                    sb.Append(this.SeparatorCharacter);
                }

                sb.Append(this.write(val));
            }

            return sb.ToString();
        }
    }
}