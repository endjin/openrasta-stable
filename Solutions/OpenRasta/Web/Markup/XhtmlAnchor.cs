#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Web.Markup
{
    using System;
    using System.Security.Principal;

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Contracts.Web.Markup.Rendering;
    using OpenRasta.DI;
    using OpenRasta.Web.Markup.Rendering;

    /// <summary>
    /// Marker class used to provide xhtml-related functionality from within pages using extension methods.
    /// </summary>
    public class XhtmlAnchor : IXhtmlAnchor
    {
        private readonly IDependencyResolver resolver;
        private readonly Func<IPrincipal> userGetter;

        public XhtmlAnchor(IDependencyResolver resolver, IXhtmlWriter writer, Func<IPrincipal> userGetter)
        {
            this.resolver = resolver;
            this.userGetter = userGetter;
            this.AmbientWriter = writer;
        }

        public IXhtmlWriter AmbientWriter { get; private set; }

        public IUriResolver Uris
        {
            get { return this.resolver.Resolve<IUriResolver>(); }
        }

        public IPrincipal User
        {
            get { return this.userGetter(); }
        }

        public IDependencyResolver Resolver
        {
            get { return this.resolver; }
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