namespace OpenRasta.OperationModel.Hydrators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using OpenRasta.Codecs;
    using OpenRasta.DI;
    using OpenRasta.Diagnostics;
    using OpenRasta.OperationModel.Hydrators.Diagnostics;
    using OpenRasta.TypeSystem.ReflectionBased;
    using OpenRasta.Web;

    public class RequestEntityReaderHydrator : IOperationHydrator
    {
        private readonly IRequest request;
        private readonly IDependencyResolver resolver;

        public RequestEntityReaderHydrator(IDependencyResolver resolver, IRequest request)
        {
            this.Log = NullLogger<CodecLogSource>.Instance;
            this.ErrorCollector = NullErrorCollector.Instance;
            this.resolver = resolver;
            this.request = request;
        }

        public IErrorCollector ErrorCollector { get; set; }

        public ILogger<CodecLogSource> Log { get; set; }

        public IEnumerable<IOperation> Process(IEnumerable<IOperation> operations)
        {
            var operation = operations.Where(x => x.GetRequestCodec() != null)
                                      .OrderByDescending(x => x.GetRequestCodec()).FirstOrDefault()
                            ?? operations.Where(x => x.Inputs.AllReady())
                                         .OrderByDescending(x => x.Inputs.CountReady()).FirstOrDefault();
            if (operation == null)
            {
                this.Log.OperationNotFound();

                yield break;
            }

            this.Log.OperationFound(operation);

            if (operation.GetRequestCodec() != null)
            {
                var codecInstance = this.CreateMediaTypeReader(operation);

                var codecType = codecInstance.GetType();
                this.Log.CodecLoaded(codecType);

                if (codecType.Implements(typeof(IKeyedValuesMediaTypeReader<>)))
                {
                    if (this.TryAssignKeyedValues(this.request.Entity, codecInstance, codecType, operation))
                    {
                        yield return operation;
                        yield break;
                    }
                }

                if (codecType.Implements<IMediaTypeReader>())
                {
                    if (!this.TryReadPayloadAsObject(this.request.Entity, (IMediaTypeReader)codecInstance, operation))
                    {
                        yield break;
                    }
                }
            }

            yield return operation;
        }

        private static ErrorFrom<RequestEntityReaderHydrator> CreateErrorForException(Exception e)
        {
            return new ErrorFrom<RequestEntityReaderHydrator>
            {
                Message = "The codec failed to process the request entity. See the exception below.\r\n" + e,
                Exception = e
            };
        }


        private ICodec CreateMediaTypeReader(IOperation operation)
        {
            return this.resolver.Resolve(operation.GetRequestCodec().CodecRegistration.CodecType, UnregisteredAction.AddAsTransient) as ICodec;
        }

        private bool TryAssignKeyedValues(IHttpEntity requestEntity, ICodec codec, Type codecType, IOperation operation)
        {
            this.Log.CodecSupportsKeyedValues();

            return codec.TryAssignKeyValues(requestEntity, operation.Inputs.Select(x => x.Binder), this.Log.KeyAssigned, this.Log.KeyFailed);
        }

        private bool TryReadPayloadAsObject(IHttpEntity requestEntity, IMediaTypeReader reader, IOperation operation)
        {
            this.Log.CodecSupportsFullObjectResolution();

            foreach (var member in from m in operation.Inputs
                                   where m.Binder.IsEmpty
                                   select m)
            {
                this.Log.ProcessingMember(member);

                try
                {
                    var entityInstance = reader.ReadFrom(requestEntity, member.Member.Type, member.Member.Name);
                    
                    this.Log.Result(entityInstance);

                    if (entityInstance != Missing.Value)
                    {
                        if (!member.Binder.SetInstance(entityInstance))
                        {
                            this.Log.BinderInstanceAssignmentFailed();
                            
                            return false;
                        }

                        this.Log.BinderInstanceAssignmentSucceeded();
                    }
                }
                catch (Exception e)
                {
                    this.ErrorCollector.AddServerError(CreateErrorForException(e));
                    
                    return false;
                }
            }

            return true;
        }
    }
}