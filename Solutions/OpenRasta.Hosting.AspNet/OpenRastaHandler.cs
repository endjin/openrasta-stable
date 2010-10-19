#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Hosting.AspNet
{
    using System.Web;

    using OpenRasta.DI;
    using OpenRasta.Diagnostics;

    public class OpenRastaHandler : IHttpHandlerFactory
    {
        private readonly IHttpHandler internalHandler;

        public OpenRastaHandler()
        {
            // detect if rewrite is necessary based on IIS6 or IIS7 being present
            // not implemented yet as I don't have an IIS6 box to test the code on (yet)
            this.internalHandler = new OpenRastaRewriterHandler();
        }

        public bool IsReusable
        {
            get { return this.internalHandler.IsReusable; }
        }

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (context.Items[OpenRastaModule.OriginalPathKey] != null)
            {
                return OpenRastaModule.HostManager.Resolver.Resolve<OpenRastaRewriterHandler>();
            }

            return OpenRastaModule.HostManager.Resolver.Resolve<OpenRastaIntegratedHandler>();
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }

    public class OpenRastaRewriterHandler : IHttpHandler
    {
        public OpenRastaRewriterHandler()
        {
            this.Log = NullLogger.Instance;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public ILogger Log { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            using (this.Log.Operation(this, "Rewriting to original path"))
            {
                HttpContext.Current.RewritePath((string)HttpContext.Current.Items[OpenRastaModule.OriginalPathKey], false);
                OpenRastaModule.HostManager.Resolver.Resolve<OpenRastaIntegratedHandler>().ProcessRequest(context);
            }
        }
    }

    public class OpenRastaIntegratedHandler : IHttpHandler
    {
        public OpenRastaIntegratedHandler()
        {
            this.Log = NullLogger.Instance;
        }
        public bool IsReusable
        {
            get { return true; }
        }

        public ILogger Log { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            using (this.Log.Operation(this, "Request for {0}".With(context.Request.Url)))
            {
                OpenRastaModule.Host.RaiseIncomingRequestReceived(OpenRastaModule.CommunicationContext);
            }
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