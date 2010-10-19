namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using System.Linq;

    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;
    using OpenRasta.Exceptions;
    using OpenRasta.Extensions;
    using OpenRasta.Web;

    #endregion

    public class ResponseEntityWriterContributor : KnownStages.IResponseCoding
    {
        private static readonly byte[] Padding = Enumerable.Repeat((byte)' ', 512).ToArray();

        public ILogger Log { get; set; }

        public void Initialize(IPipeline pipeline)
        {
            pipeline.Notify(this.WriteResponse).After<KnownStages.ICodecResponseSelection>();
        }

        public PipelineContinuation WriteResponse(ICommunicationContext context)
        {
            if (context.Response.Entity.Instance == null)
            {
                this.Log.WriteDebug("There was no response entity, not rendering.");
            }
            else
            {
                var codecInstance = context.Response.Entity.Codec as IMediaTypeWriter;
                
                if (codecInstance != null)
                {
                    this.Log.WriteDebug("Codec instance with type {0} has already been defined.", codecInstance.GetType().Name);
                }
                else
                {
                    context.Response.Entity.Codec = codecInstance = DependencyManager.GetService(context.PipelineData.ResponseCodec.CodecType) as IMediaTypeWriter;
                }

                if (codecInstance == null)
                {
                    context.ServerErrors.Add(
                        new Error
                            {
                                Title = "Codec {0} couldn't be initialized. Ensure the codec implements {1}.".With(
                                        context.PipelineData.ResponseCodec.CodecType, typeof(IMediaTypeReader).Name)
                            });

                    return PipelineContinuation.Abort;
                }

                this.Log.WriteDebug("Codec {0} selected.", codecInstance.GetType().Name);
                
                if (context.PipelineData.ResponseCodec != null && context.PipelineData.ResponseCodec.Configuration != null)
                {
                    codecInstance.Configuration = context.PipelineData.ResponseCodec.Configuration;
                }

                using (this.Log.Operation(this, "Generating response entity."))
                {
                    codecInstance.WriteTo(
                        context.Response.Entity.Instance,
                        context.Response.Entity,
                        context.Request.CodecParameters.ToArray());
                    
                    PadErrorMessageForIE(context);

                    this.Log.WriteDebug("Setting Content-Length to {0}", context.Response.Entity.Stream.Length);
                    
                    context.Response.Entity.ContentLength = context.Response.Entity.Stream.Length;
                }
            }

            this.SendResponseHeaders(context);
            
            return PipelineContinuation.Finished;
        }

        private static void PadErrorMessageForIE(ICommunicationContext context)
        {
            // IE display "friendly" messages for http errors unless the content sent is more than 512 bytes.
            if (context.OperationResult.IsClientError || context.OperationResult.IsServerError)
            {
                if (context.Response.Entity.Stream.Length <= 512 && context.Response.Entity.ContentType == MediaType.Html)
                {
                    context.Response.Entity.Stream.Write(Padding, 0, (int)(512 - context.Response.Entity.Stream.Length));
                }
            }
        }

        private void SendResponseHeaders(ICommunicationContext context)
        {
            this.Log.WriteDebug("Writing http headers.");
            
            context.Response.WriteHeaders();
        }
    }
}