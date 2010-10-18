﻿#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Configuration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using OpenRasta.Binding;
    using OpenRasta.Codecs;
    using OpenRasta.CodeDom.Compiler;
    using OpenRasta.Collections;
    using OpenRasta.Configuration.MetaModel;
    using OpenRasta.Configuration.MetaModel.Handlers;
    using OpenRasta.DI;
    using OpenRasta.Diagnostics;
    using OpenRasta.Handlers;
    using OpenRasta.OperationModel;
    using OpenRasta.OperationModel.CodecSelectors;
    using OpenRasta.OperationModel.Filters;
    using OpenRasta.OperationModel.Hydrators;
    using OpenRasta.OperationModel.Interceptors;
    using OpenRasta.OperationModel.MethodBased;
    using OpenRasta.Pipeline;
    using OpenRasta.Pipeline.Contributors;
    using OpenRasta.TypeSystem;
    using OpenRasta.TypeSystem.ReflectionBased;
    using OpenRasta.TypeSystem.Surrogated;
    using OpenRasta.TypeSystem.Surrogates;
    using OpenRasta.TypeSystem.Surrogates.Static;
    using OpenRasta.Web;

    #endregion

    public class DefaultDependencyRegistrar : IDependencyRegistrar
    {
        public DefaultDependencyRegistrar()
        {
            this.CodecTypes = new List<Type>();
            this.PipelineContributorTypes = new List<Type>();
            this.CodeSnippetModifierTypes = new List<Type>();
            this.TraceSourceListenerTypes = new List<Type>();
            this.MetaModelHandlerTypes = new List<Type>();
            this.MethodFilterTypes = new List<Type>();
            this.OperationFilterTypes = new List<Type>();
            this.OperationHydratorTypes = new List<Type>();
            this.OperationCodecSelectorTypes = new List<Type>();
            this.SurrogateBuilders = new List<Type>();
            this.LogSourceTypes = new List<Type>();

            this.SetTypeSystem<ReflectionBasedTypeSystem>();
            this.SetMetaModelRepository<MetaModelRepository>();
            this.SetUriResolver<TemplatedUriResolver>();
            this.SetCodecRepository<CodecRepository>();
            this.SetHandlerRepository<HandlerRepository>();
            this.SetPipeline<PipelineRunner>();
            this.SetLogger<TraceSourceLogger>();
            this.SetErrorCollector<OperationContextErrorCollector>();
            this.SetObjectBinderLocator<DefaultObjectBinderLocator>();
            this.SetOperationCreator<MethodBasedOperationCreator>();
            this.SetOperationExecutor<OperationExecutor>();
            this.SetOperationInterceptorProvider<SystemAndAttributesOperationInterceptorProvider>();
            this.SetPathManager<PathManager>();

            this.AddMethodFilter<TypeExclusionMethodFilter<object>>();

            this.AddDefaultCodecs();
            this.AddDefaultContributors();
            this.AddCSharpCodeSnippetModifiers();
            this.AddDefaultMetaModelHandlers();
            this.AddOperationFilters();
            this.AddOperationHydrators();
            this.AddOperationCodecResolvers();
            this.AddLogSources();
            this.AddSurrogateBuilders();
        }

        protected Type CodecRepositoryType { get; set; }

        protected IList<Type> CodecTypes { get; private set; }

        protected IList<Type> CodeSnippetModifierTypes { get; private set; }

        protected Type ErrorCollectorType { get; set; }

        protected Type HandlerRepositoryType { get; set; }

        protected Type LoggerType { get; set; }

        protected Type LogSourcedLoggerType { get; set; }

        protected IList<Type> LogSourceTypes { get; set; }

        protected IList<Type> MetaModelHandlerTypes { get; private set; }

        protected Type MetaModelRepositoryType { get; set; }

        protected IList<Type> MethodFilterTypes { get; set; }

        protected IList<Type> OperationCodecSelectorTypes { get; set; }

        protected Type OperationCreatorType { get; set; }

        protected Type OperationExecutorType { get; set; }

        protected IList<Type> OperationFilterTypes { get; set; }

        protected IList<Type> OperationHydratorTypes { get; set; }

        protected Type OperationInterceptorProviderType { get; set; }

        protected Type ParameterBinderLocatorType { get; set; }

        protected Type PathManagerType { get; set; }

        protected IList<Type> PipelineContributorTypes { get; private set; }

        protected Type PipelineType { get; set; }

        protected IList<Type> SurrogateBuilders { get; private set; }

        protected IList<Type> TraceSourceListenerTypes { get; private set; }

        protected Type TypeSystemType { get; set; }

        protected Type UriResolverType { get; set; }

        public void AddSurrogateBuilders()
        {
            this.SurrogateBuilders.Add(typeof(ListIndexerSurrogateBuilder));
            this.SurrogateBuilders.Add(typeof(DateTimeSurrogate));
        }

        public void AddCodec<T>() where T : ICodec
        {
            this.CodecTypes.Add(typeof(T));
        }

        public void AddCodeSnippetModifier<T>() where T : ICodeSnippetModifier
        {
            this.CodeSnippetModifierTypes.Add(typeof(T));
        }

        public void AddMetaModelHandler<T>() where T : IMetaModelHandler
        {
            this.MetaModelHandlerTypes.Add(typeof(T));
        }

        public void AddMethodFilter<T>() where T : IMethodFilter
        {
            this.MethodFilterTypes.Add(typeof(T));
        }

        public void AddOperationCodecSelector<T>()
        {
            this.OperationCodecSelectorTypes.Add(typeof(T));
        }

        public void AddPipelineContributor<T>() where T : IPipelineContributor
        {
            this.PipelineContributorTypes.Add(typeof(T));
        }

        public void SetCodecRepository<T>() where T : ICodecRepository
        {
            this.CodecRepositoryType = typeof(T);
        }

        public void SetErrorCollector<T>()
        {
            this.ErrorCollectorType = typeof(T);
        }

        public void SetHandlerRepository<T>() where T : IHandlerRepository
        {
            this.HandlerRepositoryType = typeof(T);
        }

        public void SetLogger<T>() where T : ILogger
        {
            this.LoggerType = typeof(T);
        }

        public void SetMetaModelRepository<T>()
        {
            this.MetaModelRepositoryType = typeof(T);
        }

        public void SetObjectBinderLocator<T>() where T : IObjectBinderLocator
        {
            this.ParameterBinderLocatorType = typeof(T);
        }

        public void SetOperationExecutor<T>()
        {
            this.OperationExecutorType = typeof(T);
        }

        public void SetPathManager<T>()
        {
            this.PathManagerType = typeof(T);
        }

        public void SetPipeline<T>() where T : IPipeline
        {
            this.PipelineType = typeof(T);
        }

        public void SetTypeSystem<T>() where T : ITypeSystem
        {
            this.TypeSystemType = typeof(T);
        }

        public void SetUriResolver<T>() where T : IUriResolver
        {
            this.UriResolverType = typeof(T);
        }

        public void Register(IDependencyResolver resolver)
        {
            this.RegisterCoreComponents(resolver);
            this.RegisterSurrogateBuilders(resolver);
            this.RegisterLogging(resolver);
            this.RegisterMetaModelHandlers(resolver);
            this.RegisterContributors(resolver);
            this.RegisterCodeSnippeModifiers(resolver);
            this.RegisterMethodFilters(resolver);
            this.RegisterOperationModel(resolver);
            this.RegisterLogSources(resolver);
            this.RegisterCodecs(resolver);
        }

        protected virtual void RegisterSurrogateBuilders(IDependencyResolver resolver)
        {
            this.SurrogateBuilders.ForEach(x => resolver.AddDependency(typeof(ISurrogateBuilder), x, DependencyLifetime.Transient));
        }

        protected void AddLogSources()
        {
            this.LogSourcedLoggerType = typeof(TraceSourceLogger<>);
            this.LogSourceTypes.AddRange(Assembly.GetExecutingAssembly().GetTypes().Where(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableTo<ILogSource>()));
        }

        protected virtual void AddOperationCodecResolvers()
        {
            AddOperationCodecSelector<RequestCodecSelector>();
        }

        protected virtual void AddOperationFilter<T>() where T : IOperationFilter
        {
            this.OperationFilterTypes.Add(typeof(T));
        }

        protected void AddOperationHydrator<T>()
        {
            this.OperationHydratorTypes.Add(typeof(T));
        }

        protected virtual void AddOperationHydrators()
        {
            AddOperationHydrator<RequestEntityReaderHydrator>();
        }

        protected virtual void RegisterCodecs(IDependencyResolver resolver)
        {
            var repo = resolver.Resolve<ICodecRepository>();
            var typeSystem = resolver.Resolve<ITypeSystem>();

            foreach (var codecType in this.CodecTypes)
            {
                if (!resolver.HasDependency(codecType))
                {
                    resolver.AddDependency(codecType, DependencyLifetime.Transient);
                }

                var registrations = CodecRegistration.FromCodecType(codecType, typeSystem);
                registrations.ForEach(repo.Add);
            }
        }

        protected virtual void RegisterCodeSnippeModifiers(IDependencyResolver resolver)
        {
            this.CodeSnippetModifierTypes.ForEach(x => resolver.AddDependency(typeof(ICodeSnippetModifier), x, DependencyLifetime.Transient));
        }

        protected virtual void RegisterContributors(IDependencyResolver resolver)
        {
            this.PipelineContributorTypes.ForEach(x => resolver.AddDependency(typeof(IPipelineContributor), x, DependencyLifetime.Singleton));
        }

        protected virtual void RegisterCoreComponents(IDependencyResolver resolver)
        {
            resolver.AddDependency(typeof(ITypeSystem), this.TypeSystemType, DependencyLifetime.Singleton);
            resolver.AddDependency(typeof(IMetaModelRepository), this.MetaModelRepositoryType, DependencyLifetime.Singleton);
            resolver.AddDependency(typeof(IUriResolver), this.UriResolverType, DependencyLifetime.Singleton);
            resolver.AddDependency(typeof(ICodecRepository), this.CodecRepositoryType, DependencyLifetime.Singleton);
            resolver.AddDependency(typeof(IHandlerRepository), this.HandlerRepositoryType, DependencyLifetime.Singleton);
            resolver.AddDependency(typeof(IPipeline), this.PipelineType, DependencyLifetime.Singleton);
            resolver.AddDependency(typeof(IObjectBinderLocator), this.ParameterBinderLocatorType, DependencyLifetime.Singleton);
            resolver.AddDependency(typeof(IOperationCreator), this.OperationCreatorType, DependencyLifetime.Transient);
            resolver.AddDependency(typeof(IOperationExecutor), this.OperationExecutorType, DependencyLifetime.Transient);
            resolver.AddDependency(typeof(IErrorCollector), this.ErrorCollectorType, DependencyLifetime.Transient);
            resolver.AddDependency(typeof(IOperationInterceptorProvider), this.OperationInterceptorProviderType, DependencyLifetime.Transient);
            resolver.AddDependency(typeof(IPathManager), this.PathManagerType, DependencyLifetime.Singleton);
            resolver.AddDependency(typeof(ISurrogateProvider), typeof(SurrogateBuilderProvider), DependencyLifetime.Singleton);
        }

        [Conditional("DEBUG")]
        protected virtual void RegisterDefaultTraceListener(IDependencyResolver resolver)
        {
            if (!resolver.HasDependencyImplementation(typeof(TraceListener), typeof(DebuggerLoggingTraceListener)))
            {
                resolver.AddDependency(typeof(TraceListener), typeof(DebuggerLoggingTraceListener), DependencyLifetime.Transient);
            }
        }

        protected virtual void RegisterLogging(IDependencyResolver resolver)
        {
            resolver.AddDependency(typeof(ILogger), this.LoggerType, DependencyLifetime.Singleton);

            this.RegisterTraceSourceLiseners(resolver);
            this.RegisterDefaultTraceListener(resolver);
        }

        protected virtual void RegisterMetaModelHandlers(IDependencyResolver resolver)
        {
            this.MetaModelHandlerTypes.ForEach(x => resolver.AddDependency(typeof(IMetaModelHandler), x, DependencyLifetime.Transient));
        }

        protected virtual void RegisterTraceSourceLiseners(IDependencyResolver resolver)
        {
            this.TraceSourceListenerTypes.ForEach(x => resolver.AddDependency(typeof(TraceListener), x, DependencyLifetime.Transient));
        }

        protected void SetOperationCreator<T>() where T : IOperationCreator
        {
            this.OperationCreatorType = typeof(T);
        }

        private void AddCSharpCodeSnippetModifiers()
        {
            AddCodeSnippetModifier<MarkupElementModifier>();
            AddCodeSnippetModifier<UnencodedOutputModifier>();
        }

        private void AddDefaultCodecs()
        {
            AddCodec<HtmlErrorCodec>();
            AddCodec<TextPlainCodec>();
            AddCodec<ApplicationXWwwFormUrlencodedKeyedValuesCodec>();
            AddCodec<ApplicationXWwwFormUrlencodedObjectCodec>();
            AddCodec<MultipartFormDataObjectCodec>();
            AddCodec<MultipartFormDataKeyedValuesCodec>();
            AddCodec<ApplicationOctetStreamCodec>();
            AddCodec<OperationResultCodec>();
        }

        private void AddDefaultContributors()
        {
            AddPipelineContributor<ResponseEntityCodecResolverContributor>();
            AddPipelineContributor<ResponseEntityWriterContributor>();
            AddPipelineContributor<BootstrapperContributor>();
            AddPipelineContributor<HttpMethodOverriderContributor>();
            AddPipelineContributor<UriDecoratorsContributor>();

            AddPipelineContributor<ResourceTypeResolverContributor>();
            AddPipelineContributor<HandlerResolverContributor>();

            AddPipelineContributor<AuthenticationContributor>();
            AddPipelineContributor<AuthenticationChallengerContributor>();

            AddPipelineContributor<OperationCreatorContributor>();
            AddPipelineContributor<OperationFilterContributor>();
            AddPipelineContributor<OperationHydratorContributor>();
            AddPipelineContributor<OperationCodecSelectorContributor>();
            AddPipelineContributor<OperationInvokerContributor>();
            AddPipelineContributor<OperationResultInvokerContributor>();

            AddPipelineContributor<OperationInterceptorContributor>();

            AddPipelineContributor<EndContributor>();
        }

        private void AddDefaultMetaModelHandlers()
        {
            AddMetaModelHandler<TypeRewriterMetaModelHandler>();
            AddMetaModelHandler<CodecMetaModelHandler>();
            AddMetaModelHandler<HandlerMetaModelHandler>();
            AddMetaModelHandler<UriRegistrationMetaModelHandler>();
            AddMetaModelHandler<DependencyRegistrationMetaModelHandler>();
        }

        private void AddOperationFilters()
        {
            AddOperationFilter<HttpMethodOperationFilter>();
            AddOperationFilter<UriNameOperationFilter>();
            AddOperationFilter<UriParametersFilter>();
        }

        private void RegisterLogSources(IDependencyResolver resolver)
        {
            this.LogSourceTypes.ForEach(x => resolver.AddDependency(typeof(ILogger<>).MakeGenericType(x), this.LogSourcedLoggerType.MakeGenericType(x), DependencyLifetime.Transient));
        }

        private void RegisterMethodFilters(IDependencyResolver resolver)
        {
            this.MethodFilterTypes.ForEach(x => resolver.AddDependency(typeof(IMethodFilter), x, DependencyLifetime.Transient));
        }

        private void RegisterOperationModel(IDependencyResolver resolver)
        {
            this.OperationFilterTypes.ForEach(x => resolver.AddDependency(typeof(IOperationFilter), x, DependencyLifetime.Transient));
            this.OperationHydratorTypes.ForEach(x => resolver.AddDependency(typeof(IOperationHydrator), x, DependencyLifetime.Transient));
            this.OperationCodecSelectorTypes.ForEach(x => resolver.AddDependency(typeof(IOperationCodecSelector), x, DependencyLifetime.Transient));
        }

        private void SetOperationInterceptorProvider<T>()
        {
            this.OperationInterceptorProviderType = typeof(T);
        }
    }
}

#region Full license
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion