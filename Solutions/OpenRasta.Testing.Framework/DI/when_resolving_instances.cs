#region License

/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */

#endregion

namespace OpenRasta.Testing.Framework.DI
{
    #region Using Directives

    using NUnit.Framework;

    using OpenRasta.Contracts.DI;
    using OpenRasta.DI;
    using OpenRasta.Exceptions;
    using OpenRasta.Testing.Framework.MockTypes;
    using OpenRasta.Testing.Specifications;

    #endregion

    public abstract class when_resolving_instances : dependency_resolver_context
    {
        public class TypeWithDependencyResolverAsProperty
        {
            public IDependencyResolver Resolver { get; set; }
        }

        public class TypeWithPropertyAlreadySet
        {
            public TypeWithPropertyAlreadySet()
            {
                Resolver = new InternalDependencyResolver();
            }
            public IDependencyResolver Resolver { get; set; }
        }
        
        [Test]
        public void a_property_that_would_cause_a_cyclic_dependency_is_ignored()
        {
            Resolver.AddDependency<RecursiveProperty>();

            Resolver.Resolve<RecursiveProperty>().Property.
                ShouldBeNull();
        }

        [Test]
        public void a_type_cannot_be_created_when_its_dependencies_are_not_registered()
        {
            Resolver.AddDependency<IAnother, Another>();

            Executing(() => Resolver.Resolve<IAnother>())
                .ShouldThrow<DependencyResolutionException>();
        }

        [Test]
        public void an_empty_enumeration_of_unregistered_types_is_resolved()
        {
            var simpleList = Resolver.ResolveAll<ISimple>();
            
            simpleList.ShouldNotBeNull();
            simpleList.ShouldBeEmpty();
        }

        [Test]
        public void a_type_can_get_a_dependency_resolver_dependency_assigned()
        {
            Resolver.AddDependencyInstance(typeof (IDependencyResolver), Resolver);
            Resolver.AddDependency<TypeWithDependencyResolverAsProperty>(DependencyLifetime.Transient);

            Resolver.Resolve<TypeWithDependencyResolverAsProperty>()
                .Resolver.ShouldBeTheSameInstanceAs(Resolver);
        }
        
        [Test]
        public void a_property_for_which_there_is_a_property_already_assigned_is_replaced_with_value_from_container()
        {
            Resolver.AddDependencyInstance(typeof(IDependencyResolver),Resolver);
            Resolver.AddDependency<TypeWithPropertyAlreadySet>(DependencyLifetime.Singleton);

            Resolver.Resolve<TypeWithPropertyAlreadySet>()
                .Resolver.ShouldBeTheSameInstanceAs(Resolver);
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
#endregion