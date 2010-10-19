namespace OpenRasta.Contracts.Codecs
{
    using System.Collections.Generic;

    using OpenRasta.Codecs.Framework;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Web;

    public interface ICodecRepository : IEnumerable<CodecRegistration>
    {
        string[] RegisteredExtensions { get; }

        void Add(CodecRegistration descriptor);

        CodecMatch FindMediaTypeReader(MediaType contentType, IEnumerable<IMember> requiredTypes, IEnumerable<IMember> optionalTypes);

        CodecRegistration FindByExtension(IMember resourceType, string extension);
        
        IEnumerable<CodecRegistration> FindMediaTypeWriter(IMember resourceType, IEnumerable<MediaType> contentTypes);
        
        void Clear();
    }
}