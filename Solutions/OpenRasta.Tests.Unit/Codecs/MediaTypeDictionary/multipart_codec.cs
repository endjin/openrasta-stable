namespace OpenRasta.Tests.Unit.Codecs.MediaTypeDictionary
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using OpenRasta.Binding;
    using OpenRasta.Codecs;
    using OpenRasta.DI;
    using OpenRasta.IO;
    using OpenRasta.Testing.Specifications;
    using OpenRasta.TypeSystem;
    using OpenRasta.Web;

    #endregion

    public class multipart_codec : media_type_reader_context<MultipartFormDataObjectCodec>
    {
        protected override MultipartFormDataObjectCodec CreateCodec(ICommunicationContext context)
        {
            return new MultipartFormDataObjectCodec(
                Context,
                DependencyManager.Codecs,
                DependencyManager.GetService<IDependencyResolver>(),
                TypeSystems.Default,
                new DefaultObjectBinderLocator());
        }

        protected void ThenTheRawResultShouldContain(params Action<MultipartHttpEntity>[] builders)
        {
            var result = then_decoding_result<IEnumerable<IMultipartHttpEntity>>();
            var entities = new List<IMultipartHttpEntity>();
        
            foreach (var builder in builders)
            {
                var expected = new MultipartHttpEntity { Stream = new MemoryStream() };

                builder(expected);
                entities.Add(expected);
            }
            
            foreach (var entity in result)
            {
                var entityData = entity.Stream.ReadToEnd();
                entities.ForEach(e => e.Stream.Position = 0);

                var foundEntity = entities.Find(fake => 
                {
                    bool headersMatch = (fake.ContentType != null ? fake.ContentType.Equals(entity.ContentType) : entity.ContentType == null)
                                                                    && fake.Headers.ContentDisposition.Equals(entity.Headers.ContentDisposition);
                    bool contentMatches = fake.Stream.ReadToEnd().SequenceEqual(entityData);
                    return headersMatch && contentMatches;
                });
                
                foundEntity.ShouldNotBeNull();
            }
        }

        protected void GivenAMultipartRequestStream(string content)
        {
            given_request_content_type("multipart/form-data;boundary=boundary42");
            given_request_stream(content);
        }
    }
}