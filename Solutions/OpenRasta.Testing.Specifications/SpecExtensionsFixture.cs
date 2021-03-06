#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Testing.Specifications
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class AssertExtensionsFixture
    {
        [Test]
        public void AssertExFailsWhenCodeThrowsAnException() { SpecExtensions.ShouldThrow<InvalidOperationException>(() => { throw new InvalidOperationException(); }); }
    }

    public class when_matching_elements_in_enumerables : context
    {
        [Test]
        public void contains_doesnt_fail_when_element_is_found()
        {
            var items = new[] {"one", "two"};
            items.ShouldContain("one");
        }

        [Test]
        public void contains_fails_when_element_is_not_found()
        {
            var items = new[] {"one", "two"};
            Executing(() => items.ShouldContain("three"))
                .ShouldThrow<AssertionException>();
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