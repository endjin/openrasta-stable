namespace OpenRasta.Testing.Framework
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using OpenRasta.Codecs;
    using OpenRasta.Collections;
    using OpenRasta.DI;
    using OpenRasta.Diagnostics;
    using OpenRasta.Handlers;
    using OpenRasta.Hosting.InMemory;
    using OpenRasta.Pipeline;
    using OpenRasta.Testing.Specifications;
    using OpenRasta.TypeSystem;
    using OpenRasta.Web;

    #endregion

    public class openrasta_context : context
    {
        private Dictionary<Type, Func<ICommunicationContext, PipelineContinuation>> actions;
        private InMemoryHost host;

        public openrasta_context()
        {
            TypeSystem = TypeSystems.Default;
        }

        protected ICodecRepository Codecs
        {
            get { return Resolver.Resolve<ICodecRepository>(); }
        }

        protected InMemoryCommunicationContext Context { get; private set; }

        protected TestErrorCollector Errors { get; private set; }

        protected bool IsContributorExecuted { get; set; }

        protected IPipeline Pipeline { get; private set; }

        protected InMemoryRequest Request
        {
            get { return Context.Request as InMemoryRequest; }
        }

        protected IDependencyResolver Resolver
        {
            get { return this.host.Resolver; }
        }

        public PipelineContinuation Result { get; set; }

        protected ITypeSystem TypeSystem { get; set; }

        protected IUriResolver UriResolver
        {
            get { return Resolver.Resolve<IUriResolver>(); }
        }

        public void given_dependency<TInterface>(TInterface instance)
        {
            Resolver.AddDependencyInstance(typeof(TInterface), instance, DependencyLifetime.Singleton);
        }

        public T given_pipeline_contributor<T>() where T : class, IPipelineContributor
        {
            return given_pipeline_contributor<T>(null);
        }

        public T given_pipeline_contributor<T>(Func<T> constructor) where T : class, IPipelineContributor
        {
            Pipeline = new SinglePipeline<T>(constructor, Resolver, this.actions);
            Pipeline.Contributors[0].Initialize(Pipeline);
            
            return (T)Pipeline.Contributors[0];
        }

        public void given_registration_urimapping<T>(string uri)
        {
            this.UriResolver.Add(new UriRegistration(uri, typeof(T).AssemblyQualifiedName, null, CultureInfo.CurrentCulture));
        }

        public void given_request_uri(string uri)
        {
            Context.Request.Uri = new Uri(uri);
        }

        public void then_contributor_is_executed()
        {
            IsContributorExecuted.ShouldBeTrue();
        }

        public PipelineContinuation when_sending_notification<TTrigger>()
        {
            IsContributorExecuted = this.actions.ContainsKey(typeof(TTrigger));
            Result = this.actions[typeof(TTrigger)](Context);
            
            return Result;
        }

        protected void given_pipeline_resourceKey<T1>()
        {
            Context.PipelineData.ResourceKey = typeof(T1).AssemblyQualifiedName;
        }

        protected void given_pipeline_selectedHandler<THandler>()
        {
            if (Context.PipelineData.SelectedHandlers == null)
            {
                Context.PipelineData.SelectedHandlers = new List<IType>();
            }

            Context.PipelineData.SelectedHandlers.Add(TypeSystem.FromClr<THandler>());
        }

        protected void given_pipeline_uriparams(NameValueCollection nameValueCollection)
        {
            if (Context.PipelineData.SelectedResource == null)
            {
                Context.PipelineData.SelectedResource = new UriRegistration(null,null);
            }

            Context.PipelineData.SelectedResource.UriTemplateParameters.Add(nameValueCollection);
        }

        protected void given_registration_codec<TCodec>()
        {
            CodecRegistration.FromCodecType(typeof(TCodec), TypeSystem).ForEach(x => Codecs.Add(x));
        }

        protected void given_registration_codec<TCodec, TResource>(string mediaTypes)
        {
            foreach (var contentType in MediaType.Parse(mediaTypes))
            {
                Codecs.Add(CodecRegistration.FromResourceType(typeof(TResource),
                                                              typeof(TCodec),
                                                              TypeSystem,
                                                              contentType,
                                                              null,
                                                              null,
                                                              false));}
        }

        protected void given_registration_handler<TResource, THandler>()
        {
            Resolver.Resolve<IHandlerRepository>().AddResourceHandler(typeof(TResource).AssemblyQualifiedName,
                                                                      TypeSystem.FromClr
                                                                          (typeof(THandler)));
        }

        protected void given_request_entity_body(byte[] bytes)
        {
            Request.Entity = new HttpEntity(new HttpHeaderDictionary(), new MemoryStream(bytes)) { ContentLength = bytes.Length };
        }

        protected void given_request_entity_body(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            Request.Entity = new HttpEntity(Request.Entity.Headers, new MemoryStream(bytes)) { ContentLength = bytes.Length };
        }

        protected void given_request_header_accept(string p)
        {
            Context.Request.Headers["Accept"] = p;
        }

        protected void given_request_header_content_type(string mediaType)
        {
            if (mediaType == null)
            {
                Context.Request.Entity.ContentType = null;
            }
            else
            {
                Context.Request.Entity.ContentType = new MediaType(mediaType);
            }
        }

        protected void given_request_header_content_type(MediaType mediaType)
        {
            Context.Request.Entity.ContentType = mediaType;
        }

        protected void given_request_httpmethod(string method)
        {
            Context.Request.HttpMethod = method;
        }

        protected void given_response_entity(object responseEntity, Type codecType, string contentType)
        {
            Context.Response.Entity.ContentType = new MediaType(contentType);

            Context.PipelineData.ResponseCodec = CodecRegistration.FromResourceType(responseEntity == null ? typeof(object) : responseEntity.GetType(),
                                                                                    codecType,
                                                                                    TypeSystem,
                                                                                    new MediaType(contentType),
                                                                                    null,
                                                                                    null,
                                                                                    false);
            given_response_entity(responseEntity);
        }

        protected void given_response_entity(object responseEntity)
        {
            Context.Response.Entity.Instance = responseEntity;
            Context.OperationResult = new OperationResult.OK { ResponseResource = responseEntity };
        }

        protected override void SetUp()
        {
            base.SetUp();
            this.host = new InMemoryHost(null);
            Pipeline = null;
            this.actions = new Dictionary<Type, Func<ICommunicationContext, PipelineContinuation>>();
            var manager = this.host.HostManager;
            Resolver.AddDependencyInstance(typeof(IErrorCollector), Errors = new TestErrorCollector());
            Resolver.AddDependency<IPathManager, PathManager>();
            
            manager.SetupCommunicationContext(Context = new InMemoryCommunicationContext());
            DependencyManager.SetResolver(Resolver);
        }

        protected override void TearDown()
        {
            base.TearDown();
            DependencyManager.UnsetResolver();
        }

        protected void given_request_uriName(string uriName)
        {
            if (Context.PipelineData.SelectedResource == null)
            {
                Context.PipelineData.SelectedResource = new UriRegistration(null, null, uriName, null);
            }
            else
            {
                var r = Context.PipelineData.SelectedResource;
                Context.PipelineData.SelectedResource = new UriRegistration(r.UriTemplate,r.ResourceKey,uriName,r.UriCulture);
            }
        }

        protected void given_context_applicationBase(string appBasePath)
        {
            Context.ApplicationBaseUri = new Uri(appBasePath,UriKind.Absolute);
        }

        private class SinglePipeline<T> : IPipeline, IPipelineExecutionOrder, IPipelineExecutionOrderAnd where T : class, IPipelineContributor
        {
            internal readonly Dictionary<Type, Func<ICommunicationContext, PipelineContinuation>> Actions;
            internal readonly List<IPipelineContributor> List;
            internal readonly IDependencyResolver resolver;
            Func<ICommunicationContext, PipelineContinuation> lastNotification;

            public SinglePipeline(Func<T> creator,
                                  IDependencyResolver resolver,
                                  Dictionary<Type, Func<ICommunicationContext, PipelineContinuation>> actions)
            {
                ContextData = new PipelineData();
                this.resolver = resolver;

                if (!this.resolver.HasDependency(typeof(T)))
                {
                    this.resolver.AddDependency<T>();
                }
                
                this.List = new List<IPipelineContributor> { creator != null ? creator() : resolver.Resolve<T>() };
                this.Actions = actions;
            }

            public IPipelineExecutionOrder And
            {
                get { return this; }
            }

            public IEnumerable<ContributorCall> CallGraph
            {
                get
                {
                    foreach (var kv in this.Actions)
                    {
                        yield return new ContributorCall { Action = kv.Value, Target = this.List[0] };
                    }
                }
            }

            public PipelineData ContextData { get; private set; }

            public IList<IPipelineContributor> Contributors
            {
                get { return this.List; }
            }

            public bool IsInitialized { get; private set; }


            public void Initialize()
            {
                IsInitialized = true;
            }

            public IPipelineExecutionOrder Notify(Func<ICommunicationContext, PipelineContinuation> notification)
            {
                this.lastNotification = notification;
                return this;
            }

            public void RegisterAsRenderStage(IPipelineContributor renderContributor)
            {
            }

            public void Run(ICommunicationContext context)
            {
            }

            public void RunCallGraph(ICommunicationContext context, bool renderNow)
            {
            }

            public IPipelineExecutionOrderAnd After(Type contributorType)
            {
                this.Actions[contributorType] = this.lastNotification;

                return this;
            }

            public IPipelineExecutionOrderAnd Before(Type contributorType)
            {
                this.Actions[contributorType] = this.lastNotification;

                return this;
            }
        }
    }
}