#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Binding
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.TypeSystem;

    public class KeyedValuesBinder : IObjectBinder
    {
        private readonly bool enumerable;
        private readonly string name;
        private readonly string typeName;

        private object cachedBuiltObject;
        private bool instanceConstructed;

        public KeyedValuesBinder(IType target) : this(target, target.Name)
        {
        }

        public KeyedValuesBinder(IType target, string name)
        {
            this.enumerable = !target.Equals<string>() && target.Type.IsEnumerable;
            this.Builder = target.CreateBuilder();
            this.name = name;
            this.typeName = target.TypeName;

            this.Prefixes = new List<string> { this.name, this.typeName };
            this.PathManager = new PathManager();
        }

        public bool IsEmpty
        {
            get { return !this.Builder.HasValue; }
        }

        public ICollection<string> Prefixes { get; private set; }

        protected ITypeBuilder Builder { get; private set; }

        protected IPathManager PathManager { get; set; }

        public virtual BindingResult BuildObject()
        {
            if (this.IsEmpty && !this.enumerable)
            {
                return BindingResult.Failure();
            }

            if (this.instanceConstructed)
            {
                return BindingResult.Success(this.cachedBuiltObject);
            }

            this.cachedBuiltObject = this.Builder.Create();
            this.instanceConstructed = true;
            
            return BindingResult.Success(this.cachedBuiltObject);
        }

        public virtual bool SetInstance(object builtInstance)
        {
            if (this.Builder.Value != null)
            {
                throw new InvalidOperationException("An instance was already set by passing a constructor key.");
            }

            this.instanceConstructed = false;

            return this.Builder.TrySetValue(builtInstance);
        }

        public bool SetProperty<TValue>(string key, IEnumerable<TValue> values, ValueConverter<TValue> converter)
        {
            this.instanceConstructed = false;
            var keyType = this.PathManager.GetPathType(this.Prefixes, key);

            bool success = keyType.Type == PathComponentType.Constructor ? SetConstructorValue(values, converter) : SetPropertyValue(key, keyType.ParsedValue, values, converter);

            if (!success)
            {
                success = SetPropertyValue(key, key, values, converter);
            }

            return success;
        }

        private bool SetConstructorValue<TValue>(IEnumerable<TValue> values, ValueConverter<TValue> converter)
        {
            return this.Builder.TrySetValue(values, converter);
        }

        private bool SetPropertyValue<TValue>(string key, string property, IEnumerable<TValue> values, ValueConverter<TValue> converter)
        {
            var propertyBuilder = this.Builder.GetProperty(property ?? key);
            
            if (propertyBuilder == null)
            {
                return false;
            }

            return propertyBuilder.TrySetValue(values, converter);
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