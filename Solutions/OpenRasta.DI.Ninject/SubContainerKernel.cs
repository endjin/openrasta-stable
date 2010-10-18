#region License

/* Authors:
 *      Aaron Lerch (aaronlerch@gmail.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Planning.Bindings;

namespace OpenRasta.DI.Ninject
{
    /// <summary>
    /// A simple "sub-container" implementation for Ninject. Resolution requests
    /// are passed to a parent as well as the child (this kernel).
    /// </summary>
    public class SubContainerKernel : StandardKernel
    {
        private readonly IKernel _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubContainerKernel"/> class.
        /// </summary>
        /// <param name="parent">The parent container.</param>
        /// <param name="modules">The modules.</param>
        public SubContainerKernel(IKernel parent, params INinjectModule[] modules) : base(modules)
        {
            _parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubContainerKernel"/> class.
        /// </summary>
        /// <param name="parent">The parent container.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="modules">The modules.</param>
        public SubContainerKernel(IKernel parent, INinjectSettings settings, params INinjectModule[] modules) : base(settings, modules)
        {
            _parent = parent;
        }

        /// <summary>
        /// Determines whether the specified request can be resolved.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// 	<c>True</c> if the request can be resolved; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanResolve(IRequest request)
        {
            var canResolve = base.CanResolve(request);
            if (!canResolve && _parent != null)
            {
                return _parent.CanResolve(request);
            }

            return canResolve;
        }

        /// <summary>
        /// Gets the bindings registered for the specified service, 
        /// aggregating the bindings from this <see cref="IKernel"/> and the parent <see cref="IKernel"/> if one was specified.
        /// </summary>
        /// <param name="service">The service in question.</param>
        /// <returns>
        /// A series of bindings that are registered for the service.
        /// </returns>
        public override IEnumerable<IBinding> GetBindings(System.Type service)
        {
            var bindings = new List<IBinding>();
            var baseBindings = base.GetBindings(service);
            if (baseBindings != null)
            {
                bindings.AddRange(baseBindings);
            }

            if (_parent != null)
            {
                var parentBindings = _parent.GetBindings(service);
                if (parentBindings != null)
                {
                    bindings.AddRange(parentBindings);
                }
            }

            return bindings;
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