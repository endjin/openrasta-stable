namespace OpenRasta.Web.Markup.Attributes.Nodes
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web.Markup.Attributes;

    #endregion

    public class XhtmlAttributeNode<T> : IAttribute<T>
    {
        private Func<string, T> reader;
        private Func<T, string> writer;
        private T value;
        private bool valueHasBeenSet;

        public XhtmlAttributeNode(string name, bool renderWhenDefault) : this(name, renderWhenDefault, null, null)
        {
        }

        public XhtmlAttributeNode(string name, bool renderWhenDefault, Func<T, string> write, Func<string, T> read)
        {
            this.Name = name;
            this.RendersOnDefaultValue = renderWhenDefault;
            this.writer = write;
            this.reader = read;
        }

        public string DefaultValue { get; set; }

        public virtual bool IsDefault
        {
            get
            {
                if (this.DefaultValue != null)
                {
                    return this.DefaultValue.Equals(this.writer(this.Value));
                }

                return this.writer(this.Value) == null;
            }
        }

        public string Name { get; set; }

        public bool RendersOnDefaultValue { get; set; }

        public string SerializedValue
        {
            get
            {
                return this.valueHasBeenSet ? this.writer(this.Value) : this.DefaultValue;
            }

            set
            {
                this.valueHasBeenSet = true;
                this.Value = this.reader(value);
            }
        }

        public T Value
        {
            get
            {
                return (!this.valueHasBeenSet && this.DefaultValue != null) ? this.reader(this.DefaultValue) : this.value;
            }

            set
            {
                this.value = value;
                this.valueHasBeenSet = true;
            }
        }

        public Func<T, string> Writer
        {
            get
            {
                return this.writer;
            }

            set
            {
                this.writer = value;
            }
        }

        public Func<string, T> Reader
        {
            get
            {
                return this.reader;
            }
            set
            {
                this.reader = value;
            }
        }

        public override string ToString()
        {
            return this.SerializedValue;
        }
    }
}