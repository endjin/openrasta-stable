namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Codecs.Framework;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Extensions;
    using OpenRasta.Pipeline;
    using OpenRasta.Web;

    #endregion

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