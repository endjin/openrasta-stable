#region License

/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Pipeline.Contributors
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Codecs;
    using OpenRasta.Diagnostics;
    using OpenRasta.Pipeline;
    using OpenRasta.TypeSystem;
    using OpenRasta.Web;

    public class ResponseEntityCodecResolverContributor : KnownStages.ICodecResponseSelection
    {
        private const string HeaderAccept = "Accept";
        private readonly ICodecRepository codecs;
        private readonly ITypeSystem typeSystem;

        public ResponseEntityCodecResolverContributor(ICodecRepository repository, ITypeSystem typeSystem)
        {
            this.codecs = repository;
            this.typeSystem = typeSystem;
        }

        public ILogger Log { get; set; }

        public PipelineContinuation FindResponseCodec(ICommunicationContext context)
        {
            if (context.Response.Entity.Instance == null || context.PipelineData.ResponseCodec != null)
            {
                this.LogNoResponseEntity();
                return PipelineContinuation.Continue;
            }

            string acceptHeader = context.Request.Headers[HeaderAccept];

            IEnumerable<MediaType> acceptedContentTypes = MediaType.Parse(string.IsNullOrEmpty(acceptHeader) ? "*/*" : acceptHeader);
            IType responseEntityType = this.typeSystem.FromInstance(context.Response.Entity.Instance);

            IEnumerable<CodecRegistration> sortedCodecs = this.codecs.FindMediaTypeWriter(responseEntityType, acceptedContentTypes);
            int codecsCount = sortedCodecs.Count();
            CodecRegistration negotiatedCodec = sortedCodecs.FirstOrDefault();

            if (negotiatedCodec != null)
            {
                this.LogCodecSelected(responseEntityType, negotiatedCodec, codecsCount);
                context.Response.Entity.ContentType = negotiatedCodec.MediaType.WithoutQuality();
                context.PipelineData.ResponseCodec = negotiatedCodec;
            }
            else
            {
                context.OperationResult = ResponseEntityHasNoCodec(acceptHeader, responseEntityType);
                return PipelineContinuation.RenderNow;
            }

            return PipelineContinuation.Continue;
        }

        public void Initialize(IPipeline pipeline)
        {
            pipeline.Notify(this.FindResponseCodec).After<KnownStages.IOperationResultInvocation>();
        }

        private static OperationResult.ResponseMediaTypeUnsupported ResponseEntityHasNoCodec(string acceptHeader, IType responseEntityType)
        {
            return new OperationResult.ResponseMediaTypeUnsupported
            {
                Title = "The response from the server could not be sent in any format understood by the UA.",
                Description =
                    string.Format(
                    "Content-type negotiation failed. Resource {0} doesn't have any codec for the content-types in the accept header:\r\n{1}",
                    responseEntityType,
                    acceptHeader)
            };
        }

        private void LogCodecSelected(IType responseEntityType, CodecRegistration negotiatedCodec, int codecsCount)
        {
            this.Log.WriteInfo(
                "Selected codec {0} out of {1} codecs for entity of type {2} and negotiated media type {3}.".With(
                    negotiatedCodec.CodecType.Name,
                    codecsCount,
                    responseEntityType.Name,
                    negotiatedCodec.MediaType));
        }

        private void LogNoResponseEntity()
        {
            this.Log.WriteInfo(
                "No response codec was searched for. The response entity is null or a response codec is already set.");
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