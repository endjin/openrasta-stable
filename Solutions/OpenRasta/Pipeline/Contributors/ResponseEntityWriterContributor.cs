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
    using System.Linq;

    using OpenRasta.Codecs;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;
    using OpenRasta.Diagnostics;
    using OpenRasta.Exceptions;
    using OpenRasta.Extensions;
    using OpenRasta.Web;

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

        private void SendResponseHeaders(ICommunicationContext context)
        {
            this.Log.WriteDebug("Writing http headers.");
            
            context.Response.WriteHeaders();
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