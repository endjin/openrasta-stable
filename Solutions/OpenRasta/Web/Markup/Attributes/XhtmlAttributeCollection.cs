namespace OpenRasta.Web.Markup.Attributes
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using OpenRasta.Collections;
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Web.Markup.Attributes.Nodes;

    #endregion

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