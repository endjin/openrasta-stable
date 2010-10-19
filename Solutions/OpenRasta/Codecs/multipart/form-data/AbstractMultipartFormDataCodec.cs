namespace OpenRasta.Codecs
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using OpenRasta.Binding;
    using OpenRasta.Codecs.Extensions;
    using OpenRasta.Collections;
    using OpenRasta.Collections.Specialized;
    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Diagnostics;
    using OpenRasta.Extensions;
    using OpenRasta.OperationModel.Hydrators.Diagnostics;
    using OpenRasta.Pipeline;
    using OpenRasta.TypeSystem;
    using OpenRasta.TypeSystem.ReflectionBased;
    using OpenRasta.Web;

    #endregion

    public abstract class AbstractMultipartFormDataCodec
    {
        private const string FormdataCache = "__MultipartFormDataCodec_FORMDATA_CACHED";

        // todo: inject the treshold from configuration, and be per-resource
        private const int RequestLengthTreshold = 80000;
        private readonly byte[] buffer = new byte[4096];
        private readonly ICodecRepository codecs;
        private readonly IDependencyResolver container;
        private readonly PipelineData pipeline;
        private readonly ITypeSystem typeSystem;

        protected AbstractMultipartFormDataCodec(
            ICommunicationContext context,
            ICodecRepository codecs,
            IDependencyResolver container, 
            ITypeSystem typeSystem, 
            IObjectBinderLocator binderLocator)
        {
            // temporary until IRequest / IResponse are moved to the container
            this.pipeline = context.PipelineData;
            this.codecs = codecs;
            this.typeSystem = typeSystem;
            this.container = container;
            this.BinderLocator = binderLocator;
            this.Log = NullLogger<CodecLogSource>.Instance;
        }

        public object Configuration { get; set; }

        public ILogger<CodecLogSource> Log { get; set; }

        protected IObjectBinderLocator BinderLocator { get; private set; }

        private IDictionary<IHttpEntity, IDictionary<string, IList<IMultipartHttpEntity>>> Cache
        {
            get
            {
                return (this.pipeline[FormdataCache] ??
                        (this.pipeline[FormdataCache] =
                         new NullBehaviorDictionary<IHttpEntity, IDictionary<string, IList<IMultipartHttpEntity>>>()))
                       as IDictionary<IHttpEntity, IDictionary<string, IList<IMultipartHttpEntity>>>;
            }
        }

        public BindingResult ConvertValues(IMultipartHttpEntity entity, Type targetType)
        {
            var sourceMediaType = entity.ContentType ?? MediaType.TextPlain;

            var type = this.typeSystem.FromClr(targetType);
            var mediaTypeReaderReg = this.codecs.FindMediaTypeReader(sourceMediaType, new[] { type }, null);
            
            if (mediaTypeReaderReg != null)
            {
                var mediaTypeReader = (ICodec)this.container.Resolve(mediaTypeReaderReg.CodecRegistration.CodecType);
                
                if (mediaTypeReader is IMediaTypeReader)
                {
                    return BindingResult.Success(((IMediaTypeReader)mediaTypeReader).ReadFrom(entity, type, targetType.Name));
                }
                
                var binder = this.BinderLocator.GetBinder(type);

                if (mediaTypeReader.TryAssignKeyValues(entity, binder))
                {
                    return binder.BuildObject();
                }
            }

            // if no media type reader was found, try to parse to a string and convert from that.
            var stringType = this.typeSystem.FromClr<string>();
            mediaTypeReaderReg = this.codecs.FindMediaTypeReader(sourceMediaType, new[] { stringType }, null);

            if (entity.ContentType == null)
            {
                entity.ContentType = MediaType.TextPlain;
            }

            // defaults the entity to UTF-8 if none is specified, to account for browsers favouring using the charset of the origin page rather than the standard. Cause RFCs are too difficult to follow uh...
            if (entity.ContentType.CharSet == null)
            {
                entity.ContentType.CharSet = "UTF-8";
            }

            var plainTextReader = (IMediaTypeReader)this.container.Resolve(mediaTypeReaderReg.CodecRegistration.CodecType);
            var targetString = plainTextReader.ReadFrom(entity, stringType, targetType.Name);
            object destination = targetType.CreateInstanceFrom(targetString);

            return BindingResult.Success(destination);
        }

        public IEnumerable<KeyedValues<IMultipartHttpEntity>> ReadKeyValues(IHttpEntity entity)
        {
            foreach (string key in this.FormData(entity).Keys.ToArray())
            {
                var kv = new KeyedValues<IMultipartHttpEntity>(key, this.FormData(entity)[key], this.ConvertValues);

                yield return kv;

                if (kv.WasUsed)
                {
                    this.FormData(entity).Remove(key);
                }
            }
        }

        // Note that we store in the pipeline data because the same codec may be called for resolving several request entities
        protected IDictionary<string, IList<IMultipartHttpEntity>> FormData(IHttpEntity source)
        {
            return this.Cache[source] ?? (this.Cache[source] = this.PreLoadAllParts(source));
        }

        private static Stream CreateTempFile(out string filePath)
        {
            filePath = Path.GetTempFileName();

            return File.OpenWrite(filePath);
        }

        private IDictionary<string, IList<IMultipartHttpEntity>> PreLoadAllParts(IHttpEntity source)
        {
            var multipartReader = new MultipartReader(source.ContentType.Boundary, source.Stream)
            {
                Log = this.Log
            };

            var formData = new NullBehaviorDictionary<string, IList<IMultipartHttpEntity>>(StringComparer.OrdinalIgnoreCase);
            
            foreach (var requestPart in multipartReader.GetParts())
            {
                if (requestPart.Headers.ContentDisposition != null &&
                    requestPart.Headers.ContentDisposition.Disposition.EqualsOrdinalIgnoreCase("form-data"))
                {
                    var memoryStream = new MemoryStream();
                    int totalRead = 0, lastRead;

                    while ((lastRead = requestPart.Stream.Read(this.buffer, 0, this.buffer.Length)) > 0)
                    {
                        totalRead += lastRead;

                        if (totalRead > RequestLengthTreshold)
                        {
                            string filePath;
                            
                            using (var fileStream = CreateTempFile(out filePath))
                            {
                                memoryStream.Position = 0;
                                memoryStream.CopyTo(fileStream);
                                fileStream.Write(this.buffer, 0, lastRead);
                                requestPart.Stream.CopyTo(fileStream);
                            }

                            memoryStream = null;
                            requestPart.SwapStream(filePath);
                            
                            break;
                        }

                        memoryStream.Write(this.buffer, 0, lastRead);
                    }

                    if (memoryStream != null)
                    {
                        memoryStream.Position = 0;
                        requestPart.SwapStream(memoryStream);
                    }

                    var listOfEntities = formData[requestPart.Headers.ContentDisposition.Name]
                                         ??
                                         (formData[requestPart.Headers.ContentDisposition.Name] = new List<IMultipartHttpEntity>());
                    
                    if (requestPart.ContentType == null)
                    {
                        requestPart.ContentType = MediaType.TextPlain;
                    }

                    listOfEntities.Add(requestPart);
                }
            }

            return formData;
        }
    }
}