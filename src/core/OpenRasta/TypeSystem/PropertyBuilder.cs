#region License

/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.TypeSystem
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Binding;

    public class PropertyBuilder : MemberBuilder, IPropertyBuilder
    {
        private object cachedValue;

        public PropertyBuilder(IMemberBuilder parent, IProperty property)
            : base(parent, property)
        {
            this.Owner = parent;
        }

        public int IndexAtCreation { get; set; }

        public IProperty Property
        {
            get { return Member as IProperty; }
        }

        public override bool CanWrite
        {
            get { return this.Property.CanWrite; }
        }

        public IMemberBuilder Owner { get; private set; }

        public override object Value
        {
            get { return this.cachedValue; }
        }

        public override bool HasValue
        {
            get { return this.cachedValue != null; }
        }

        public override bool TrySetValue(object value)
        {
            if (!this.Property.CanSetValue(value))
            {
                return false;
            }

            this.cachedValue = value;
            
            return true;
        }

        /// <summary>
        /// Tries to assign a property value and return <c>true</c> if it was successfully assigned or if the parent wasn't available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public override bool TrySetValue<T>(IEnumerable<T> values, ValueConverter<T> converter)
        {
            if (this.Property.Type.IsEnumerable && this.cachedValue != null)
            {
                var addMethod = this.Property.Type.GetMethod("Add");
                
                if (addMethod != null)
                {
                    var parameter = addMethod.InputMembers.FirstOrDefault();
                    var builder = parameter.Type.CreateBuilder();
                    
                    if (builder.TrySetValue(values, converter))
                    {
                        addMethod.Invoke(this.cachedValue, builder.Value);
                        
                        return true;
                    }
                }
            }

            object newValue;
            
            var success = this.Property.Type.TryCreateInstance(values, converter, out newValue);
            
            if (success)
            {
                this.cachedValue = newValue;
            }

            return success;
        }

        public override object Apply(object target, out object assignedValue)
        {
            if (target == null)
            {
                target = this.Owner.Member.Type.CreateInstance();
            }

            if (this.cachedValue != null)
            {
                this.Property.TrySetValue(target, assignedValue = this.cachedValue);
            }
            else
            {
                assignedValue = this.Property.GetValue(target);
            }

            return target;
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