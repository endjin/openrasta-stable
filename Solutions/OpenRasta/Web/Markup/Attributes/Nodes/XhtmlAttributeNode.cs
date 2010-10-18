#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Web.Markup.Attributes.Nodes
{
    using System;

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

#region Full license
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion