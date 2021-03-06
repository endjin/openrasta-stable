namespace OpenRasta.Codecs.Framework
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Collections;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Web;

    #endregion

    public class CodecRegistration : IEquatable<CodecRegistration>
    {
        public CodecRegistration(Type codecType, object resourceKey, MediaType mediaType)
            : this(codecType, resourceKey, false, mediaType, null, null, false)
        {
        }

        public CodecRegistration(
            Type codecType,
            object resourceKey,
            bool strictRegistration, 
            MediaType mediaType, 
            IEnumerable<string> extensions, 
            object codecConfiguration, 
            bool system)
        {
            CheckArgumentsAreValid(codecType, resourceKey, mediaType, strictRegistration);
            this.ResourceKey = resourceKey;
            MediaType = mediaType;
            this.CodecType = codecType;
            this.Extensions = new List<string>();
            
            if (extensions != null)
            {
                this.Extensions.AddRange(extensions);
            }

            this.Configuration = codecConfiguration;
            this.IsSystem = system;
            this.IsStrict = strictRegistration;
        }

        public Type CodecType { get; private set; }

        public object Configuration { get; private set; }

        public IList<string> Extensions { get; private set; }

        public bool IsStrict { get; private set; }

        /// <summary>
        /// Defines if the codec is to be preserved between configuration refreshes because it is part of the
        /// OpenRasta framework.
        /// </summary>
        public bool IsSystem { get; private set; }

        public MediaType MediaType { get; private set; }

        public object ResourceKey { get; private set; }

        public IType ResourceType
        {
            get { return this.ResourceKey as IType; }
        }

        public static IEnumerable<CodecRegistration> FromCodecType(Type codecType, ITypeSystem typeSystem)
        {
            var resourceTypeAttributes = codecType.GetCustomAttributes(typeof(SupportedTypeAttribute), true).Cast<SupportedTypeAttribute>();
            var mediaTypeAttributes = codecType.GetCustomAttributes(typeof(MediaTypeAttribute), true).Cast<MediaTypeAttribute>();
            
            return from resourceTypeAttribute in resourceTypeAttributes
                   from mediaType in mediaTypeAttributes
                   let isStrictRegistration = IsStrictRegistration(resourceTypeAttribute.Type)
                   let resourceType = isStrictRegistration ? GetStrictType(resourceTypeAttribute.Type) : resourceTypeAttribute.Type
                   select new CodecRegistration(
                       codecType, 
                       typeSystem.FromClr(resourceType), 
                       isStrictRegistration, 
                       mediaType.MediaType, 
                       mediaType.Extensions, 
                       null, 
                       true);
        }

        public static CodecRegistration FromResourceType(
            Type resourceType,
            Type codecType,
            ITypeSystem typeSystem, 
            MediaType mediaType, 
            IEnumerable<string> extensions, 
            object codecConfiguration, 
            bool system)
        {
            bool strict = false;
            
            if (IsStrictRegistration(resourceType))
            {
                resourceType = GetStrictType(resourceType);
                strict = true;
            }
            
            return new CodecRegistration(
                codecType, 
                typeSystem.FromClr(resourceType), 
                strict, 
                mediaType, 
                extensions, 
                codecConfiguration, 
                system);
        }

        public static Type GetStrictType(Type registration)
        {
            return registration.GetGenericArguments()[0];
        }

        public static bool IsStrictRegistration(Type type)
        {
            return type.IsGenericType
                   && type.GetGenericTypeDefinition() == typeof(Strictly<>);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(CodecRegistration))
            {
                return false;
            }

            return this.Equals((CodecRegistration)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.ResourceKey != null ? this.ResourceKey.GetHashCode() : 0;
                result = (result * 397) ^ (this.CodecType != null ? this.CodecType.GetHashCode() : 0);
                result = (result * 397) ^ (MediaType != null ? MediaType.GetHashCode() : 0);
                result = (result * 397) ^ this.IsStrict.GetHashCode();
                result = (result * 397) ^ (this.Extensions != null ? this.Extensions.GetHashCode() : 0);
                result = (result * 397) ^ (this.Configuration != null ? this.Configuration.GetHashCode() : 0);
                result = (result * 397) ^ this.IsSystem.GetHashCode();

                return result;
            }
        }

        public bool Equals(CodecRegistration other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.ResourceKey, this.ResourceKey) && 
                   Equals(other.CodecType, this.CodecType) && 
                   Equals(other.MediaType, this.MediaType) && 
                   other.IsStrict.Equals(this.IsStrict) &&
                   Equals(other.Extensions, this.Extensions) && 
                   Equals(other.Configuration, this.Configuration) && 
                   other.IsSystem.Equals(this.IsSystem);
        }

        /// <exception cref="ArgumentException">Cannot do a strict registration on resources with keys that are not types.</exception>
        /// <exception cref="ArgumentNullException"><c>mediaType</c> is null.</exception>
        private static void CheckArgumentsAreValid(Type codecType, 
                                           object resourceKey, 
                                           MediaType mediaType, 
                                           bool isStrictRegistration)
        {
            if (codecType == null)
            {
                throw new ArgumentNullException("codecType", "codecType is null.");
            }

            if (resourceKey == null)
            {
                throw new ArgumentNullException("resourceKey", "resourceKey is null.");
            }
            
            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType", "mediaType is null.");
            }
            
            if (resourceKey is Type)
            {
                throw new ArgumentException("If using a type as a resourceKey, use an IType instead.", "resourceKey");
            }
            
            if (isStrictRegistration && !(resourceKey is IType))
            {
                throw new ArgumentException(
                    "Cannot do a strict registration on resources with keys that are not types.", "isStrictRegistration");
            }
        }
    }
}