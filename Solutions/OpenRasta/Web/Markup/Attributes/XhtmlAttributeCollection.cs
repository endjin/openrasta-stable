﻿#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Web.Markup.Attributes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using OpenRasta.Collections;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    public class XhtmlAttributeCollection : IAttributeCollection
    {
        private readonly NullBehaviorDictionary<string, IAttribute> attributes = new NullBehaviorDictionary<string, IAttribute>();

        public IDictionary<string, Func<IAttribute>> AllowedAttributes { get; set; }

        public int Count
        {
            get { return this.attributes.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IAttribute this[string key]
        {
            get
            {
                if (this.attributes[key] == null && this.AllowedAttributes != null)
                {
                    if (this.AllowedAttributes.ContainsKey(key))
                    {
                        this.attributes[key] = this.AllowedAttributes[key]();
                    }
                    else
                    {
                        this.attributes[key] = new PrimaryTypeAttributeNode<string>(key);
                    }
                }

                return this.attributes[key];
            }

            set
            {
                this.attributes[key] = value;
            }
        }

        IAttribute IList<IAttribute>.this[int index]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public string GetAttribute(string attributeName)
        {
            return GetAttribute<string>(attributeName);
        }

        public T GetAttribute<T>(string attributeName)
        {
            IAttribute attrib;
            
            if (!this.attributes.TryGetValue(attributeName, out attrib))
            {
                this.attributes.Add(attributeName, attrib = CreateAttribute<T>(attributeName));
            }

            return ((IAttribute<T>)attrib).Value;
        }

        public void SetAttribute<T>(string attributeName, T value)
        {
            IAttribute attrib;
            
            if (!this.attributes.TryGetValue(attributeName, out attrib))
            {
                this.attributes.Add(attributeName, attrib = CreateAttribute<T>(attributeName));
            }

            ((IAttribute<T>)attrib).Value = value;
        }

        public void Add(IAttribute item)
        {
            this.attributes.Add(item.Name, item);
        }

        public void Clear()
        {
            this.attributes.Clear();
        }

        public bool Contains(IAttribute item)
        {
            return this.attributes.ContainsKey(item.Name);
        }

        public void CopyTo(IAttribute[] array, int arrayIndex)
        {
            this.attributes.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(IAttribute item)
        {
            return this.attributes.Remove(item.Name);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<IAttribute> GetEnumerator()
        {
            return this.attributes.Values.GetEnumerator();
        }

        public int IndexOf(IAttribute item)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, IAttribute item)
        {
            this.attributes.Add(item.Name, item);
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        private IAttribute<T> CreateAttribute<T>(string name)
        {
            Type attribType = typeof(T);
            
            if (this.AllowedAttributes != null && this.AllowedAttributes.ContainsKey(name))
            {
                return (IAttribute<T>)this.AllowedAttributes[name]();
            }
            
            //if (!dynamicAttributesPermitted)
            //    throw new ArgumentOutOfRangeException("name", "Attribute {0} is not allowed on this element.".With(name));

            if (attribType.IsValueType || attribType == typeof(string) || typeof(Nullable).IsAssignableFrom(attribType))
            {
                return new PrimaryTypeAttributeNode<T>(name);
            }
            
            if (attribType == typeof(MediaType))
            {
                return (IAttribute<T>)new XhtmlAttributeNode<MediaType>(name, false, media => media.ToString(), str => new MediaType(str));
            }
            
            if (attribType == typeof(IList<Uri>))
            {
                return (IAttribute<T>)new CharacterSeparatedAttributeNode<Uri>(name, " ", uri => uri.ToString(), s => new Uri(s, UriKind.Absolute));
            }
            
            if (attribType == typeof(IList<MediaType>))
            {
                return (IAttribute<T>)new CharacterSeparatedAttributeNode<MediaType>(name, " ", mediatype => mediatype.ToString(), str => new MediaType(str));
            }
            
            if (attribType == typeof(IList<string>))
            {
                return (IAttribute<T>)new CharacterSeparatedAttributeNode<string>(name, " ", i => i, i => i);
            }
            
            throw new InvalidOperationException("Could not automatically create attribute of type " + typeof(T));
        }
    }
}

#region Full license
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion