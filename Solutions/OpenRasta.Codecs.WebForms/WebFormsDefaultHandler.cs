#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

using System.Web.Compilation;
using System.Web.UI;
using OpenRasta.DI;
using OpenRasta.TypeSystem;
using OpenRasta.Web;

namespace OpenRasta.Codecs.WebForms
{
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;

    public class WebFormsDefaultHandler
    {
        readonly ICommunicationContext _context;
        readonly ITypeSystem _typeSystem;
        readonly IType _pageType;
        readonly IDependencyResolver _resolver;

        public WebFormsDefaultHandler(ICommunicationContext context, ITypeSystem typeSystem, IDependencyResolver resolver)
        {
            _context = context;
            _typeSystem = typeSystem;
            _resolver = resolver;
            _pageType = typeSystem.FromClr<Page>();
        }

        public OperationResult Get()
        {
            var resourceKey = _context.PipelineData.ResourceKey as IType;

            if (resourceKey == null && _context.PipelineData.ResourceKey is string)
            {
                try
                {
                    resourceKey = _typeSystem.FromClr( BuildManager.GetCompiledType((string)_context.PipelineData.ResourceKey));
                }
                catch
                {
                }
            }

            if (resourceKey != null && resourceKey.CompareTo(_pageType) >= 0)
            {
                return new OperationResult.OK
                    {
                        ResponseResource = resourceKey.CreateInstance(_resolver)
                    };
            }
            return new OperationResult.NotFound();
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