namespace OpenRasta.Codecs.Framework
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Collections;
    using OpenRasta.Collections.Specialized;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.TypeSystem.ReflectionBased;
    using OpenRasta.Web;

    #endregion

    public class CodecRepository : ICodecRepository
    {
        private readonly MediaTypeDictionary<CodecRegistration> codecs = new MediaTypeDictionary<CodecRegistration>();

        public string[] RegisteredExtensions
        {
            get { return this.codecs.SelectMany(reg => reg.Extensions).ToArray(); }
        }

        public void Add(CodecRegistration codecRegistration)
        {
            this.codecs.Add(codecRegistration.MediaType, codecRegistration);
        }

        public void Clear()
        {
        }

        public CodecRegistration FindByExtension(IMember resourceMember, string extension)
        {
            foreach (var codecRegistration in this.codecs)
            {
                var codecResourceType = codecRegistration.ResourceType;

                if (codecRegistration.Extensions.Contains(extension, StringComparison.OrdinalIgnoreCase))
                {
                    if (codecRegistration.IsStrict && resourceMember.Type.CompareTo(codecResourceType) == 0)
                    {
                        return codecRegistration;
                    }

                    if (resourceMember.Type.CompareTo(codecResourceType) >= 0)
                    {
                        return codecRegistration;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Selects the best codec for a given media type and a set of parameters to be resolved.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="requiredTypes"></param>
        /// <param name="optionalTypes"></param>
        /// <returns>The codec registration and score matching the list of parameters.</returns>
        /// <remarks>
        /// <para>The score is calculated as the average distance of the codec to the parameter types.</para>
        /// <para>For example, if Customer inherits directly from Object, the distance between Object and Customer is 1,and the distance between Customer and itself is 0.</para>
        /// </remarks>
        public CodecMatch FindMediaTypeReader(MediaType mediaType, IEnumerable<IMember> required, IEnumerable<IMember> optional)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType");
            }

            if (required == null)
            {
                throw new ArgumentNullException("required");
            }
            
            var codecMatches = new List<CodecMatch>();
            var readerCodecs = from codec in this.codecs.Matching(mediaType)
                               where codec.CodecType.Implements<IMediaTypeReader>() ||
                                     codec.CodecType.Implements(typeof(IKeyedValuesMediaTypeReader<>))
                               select codec;

            foreach (var codec in readerCodecs)
            {
                float totalDistanceToRequiredParameters = 0;
                
                if (required.Any())
                {
                    totalDistanceToRequiredParameters = this.CalculateScoreFor(required, codec);
                    if (totalDistanceToRequiredParameters == -1)
                    {
                        continue; // the codec cannot resolve the required parameters
                    }
                }

                int totalDistanceToOptionalParameters = 0;
                int totalOptionalParametersCompatibleWithCodec = 0;

                if (optional != null)
                {
                    foreach (var optionalType in optional)
                    {
                        int typeScore = CalculateDistance(optionalType, codec);

                        if (typeScore > -1)
                        {
                            totalDistanceToOptionalParameters += typeScore;
                            totalOptionalParametersCompatibleWithCodec++;
                        }
                    }
                }

                float averageScore = totalDistanceToRequiredParameters + totalDistanceToOptionalParameters;

                codecMatches.Add(
                    new CodecMatch(
                        codec, averageScore, required.Count() + totalOptionalParametersCompatibleWithCodec));
            }

            if (codecMatches.Count == 0)
            {
                return null;
            }

            codecMatches.Sort();
            codecMatches.Reverse();
            
            return codecMatches[0];
        }

        public IEnumerable<CodecRegistration> FindMediaTypeWriter(IMember resourceType, IEnumerable<MediaType> requestedMediaTypes)
        {
            var orderedMediaTypes = requestedMediaTypes.OrderByDescending(mt => mt);
            var mediaTypesByQuality = orderedMediaTypes.GroupBy(key => key.Quality);

            return mediaTypesByQuality.Aggregate(new List<CodecRegistration>(), this.AppendMediaTypeWriterFor(resourceType));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.codecs.Distinct().GetEnumerator();
        }

        IEnumerator<CodecRegistration> IEnumerable<CodecRegistration>.GetEnumerator()
        {
            return this.codecs.Distinct().GetEnumerator();
        }

        private static int CalculateDistance(IMember member, CodecRegistration registration)
        {
            if (registration.ResourceType == null)
            {
                return -1;
            }

            if (registration.IsStrict)
            {
                return (member.Type.CompareTo(registration.ResourceType) == 0) ? 0 : -1;
            }

            return member.Type.CompareTo(registration.ResourceType);
        }

        private Func<IEnumerable<CodecRegistration>, IGrouping<float, MediaType>, IEnumerable<CodecRegistration>> AppendMediaTypeWriterFor(IMember resourceType)
        {
            return (source, mediaTypes) => source.Concat(this.FindMediaTypeWriterFor(mediaTypes, resourceType));
        }

        private float CalculateScoreFor(IEnumerable<IMember> types, CodecRegistration registration)
        {
            float score = 0;

            foreach (var requestedType in types)
            {
                int typeComparison = CalculateDistance(requestedType.Type, registration);
                
                if (typeComparison == -1)
                {
                    return -1;
                }
                
                float typeScore = 1f / (1f + typeComparison);
                score += typeScore;
            }

            return score;
        }

        private IEnumerable<CodecRegistration> FindMediaTypeWriterFor(IEnumerable<MediaType> mediaTypes, IMember resourceType)
        {
            return from mediaType in mediaTypes
                   from codec in this.codecs.Matching(mediaType)
                   where codec.CodecType.Implements<IMediaTypeWriter>()
                   let match = new CodecMatch(codec, this.CalculateScoreFor(new[] { resourceType }, codec), int.MaxValue)
                   where match.Score >= 0
                   orderby match descending
                   select codec;
        }
    }
}