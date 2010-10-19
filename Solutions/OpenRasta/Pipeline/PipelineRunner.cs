#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    using OpenRasta.Collections.Specialized;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;
    using OpenRasta.Diagnostics;
    using OpenRasta.Exceptions;
    using OpenRasta.Extensions;
    using OpenRasta.Pipeline.Diagnostics;
    using OpenRasta.Web;

    public class PipelineRunner : IPipeline
    {
        private readonly IList<IPipelineContributor> contributors = new List<IPipelineContributor>();
        private readonly ICollection<Notification> notificationRegistrations = new List<Notification>();
        private readonly IDependencyResolver resolver;
        private IEnumerable<ContributorCall> callGraph;

        public PipelineRunner(IDependencyResolver resolver)
        {
            this.Contributors = new ReadOnlyCollection<IPipelineContributor>(this.contributors);
            this.resolver = resolver;
            this.PipelineLog = NullLogger<PipelineLogSource>.Instance;
            this.Log = NullLogger.Instance;
        }

        public IList<IPipelineContributor> Contributors { get; private set; }

        public bool IsInitialized { get; private set; }

        public ILogger<PipelineLogSource> PipelineLog { get; set; }

        public ILogger Log { get; set; }

        IEnumerable<ContributorCall> IPipeline.CallGraph
        {
            get { return this.callGraph; }
        }

        private void CheckPipelineIsInitialized()
        {
            if (!this.IsInitialized)
            {
                throw new InvalidOperationException("The pipeline has not been initialized and cannot run.");
            }
        }

        public void Initialize()
        {
            if (this.IsInitialized)
            {
                return;
            }

            using (this.PipelineLog.Operation(this, "Initializing the pipeline."))
            {
                foreach (var item in this.resolver.ResolveAll<IPipelineContributor>())
                {
                    this.PipelineLog.WriteDebug("Initialized contributor {0}.", item.GetType().Name);
                    this.contributors.Add(item);
                }

                this.callGraph = this.GenerateCallGraph();
            }

            this.IsInitialized = true;

            this.PipelineLog.WriteInfo("Pipeline has been successfully initialized.");
        }

        public IPipelineExecutionOrder Notify(Func<ICommunicationContext, PipelineContinuation> action)
        {
            if (this.IsInitialized)
            {
                this.PipelineLog.WriteWarning("A pipeline registration through Notify() has been done after the pipeline was initialized. Ignoring.");
                
                return new Notification(this, action);
            }

            var notification = new Notification(this, action);
            
            this.notificationRegistrations.Add(notification);

            return notification;
        }

        public void Run(ICommunicationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.CheckPipelineIsInitialized();

            if (context.PipelineData.PipelineStage == null)
            {
                context.PipelineData.PipelineStage = new PipelineStage(this);
            }

            this.RunCallGraph(context, context.PipelineData.PipelineStage);
        }

        private void RunCallGraph(ICommunicationContext context, PipelineStage stage)
        {
            lock (stage)
            {
                foreach (var contrib in stage)
                {
                    if (!this.CanBeExecuted(contrib))
                    {
                        continue;
                    }

                    stage.CurrentState = this.ExecuteContributor(context, contrib);
                    
                    switch (stage.CurrentState)
                    {
                        case PipelineContinuation.Abort:
                            this.AbortPipeline(context);
                            goto case PipelineContinuation.RenderNow;
                        case PipelineContinuation.RenderNow:
                            this.RenderNow(context, stage);
                            break;
                        case PipelineContinuation.Finished:
                            this.FinishPipeline(context);
                            return;
                    }
                }
            }
        }

        private void RenderNow(ICommunicationContext context, PipelineStage stage)
        {
            this.PipelineLog.WriteDebug("Pipeline is in RenderNow mode.");
            
            if (!stage.ResumeFrom<KnownStages.IOperationResultInvocation>())
            {
                if (stage.OwnerStage != null)
                {
                    this.PipelineLog.WriteError("Trying to launch nested pipeline to render error failed.");
                    AttemptCatastrophicErrorNotification(context);
                    
                    return;
                }

                using (this.PipelineLog.Operation(this, "Rendering contributor has already been executed. Calling a nested pipeline to render the error."))
                {
                    var nestedPipeline = new PipelineStage(this, stage);
                    if (!nestedPipeline.ResumeFrom<KnownStages.IOperationResultInvocation>())
                    {
                        throw new InvalidOperationException(
                            "Could not find an IOperationResultInvocation in the new pipeline.");
                    }

                    this.RunCallGraph(context, nestedPipeline);
                }
            }
        }

        private static void AttemptCatastrophicErrorNotification(ICommunicationContext context)
        {
            try
            {
                string fatalError = "An error in one of the rendering components of OpenRasta prevents the error message from being sent back.";

                context.Response.StatusCode = 500;
                context.Response.Entity.ContentLength = fatalError.Length;
                context.Response.Entity.Stream.Write(Encoding.ASCII.GetBytes(fatalError), 0, fatalError.Length);
                context.Response.WriteHeaders();
            }
            catch
            {
            }
        }

        private bool CanBeExecuted(ContributorCall call)
        {
            if (call.Action == null)
            {
                this.PipelineLog.WriteWarning("Contributor call for {0} had a null Action.", call.ContributorTypeName);
                
                return false;
            }

            return true;
        }

        protected virtual void AbortPipeline(ICommunicationContext context)
        {
            this.PipelineLog.WriteError("Aborting the pipeline and rendering the errors.");

            context.OperationResult = new OperationResult.InternalServerError
            {
                Title = "The request could not be processed because of a fatal error. See log below.",
                ResponseResource = context.ServerErrors
            };

            context.PipelineData.ResponseCodec = null;
            context.Response.Entity.Instance = context.ServerErrors;
            context.Response.Entity.Codec = null;
            context.Response.Entity.ContentLength = null;

            this.Log.WriteError(
                "An error has occurred and the processing of the request has stopped.\r\n{0}", 
                context.ServerErrors.Aggregate(string.Empty, (str, error) => str + "\r\n" + error.ToString()));
        }

        protected virtual PipelineContinuation ExecuteContributor(ICommunicationContext context, ContributorCall call)
        {
            using (this.PipelineLog.Operation(this, "Executing contributor {0}.{1}".With(call.ContributorTypeName, call.Action.Method.Name)))
            {
                PipelineContinuation nextStep;

                try
                {
                    nextStep = call.Action(context);
                }
                catch (Exception e)
                {
                    context.ServerErrors.Add(new Error
                    {
                        Title = "Fatal error",
                        Message = "An exception was thrown while processing a pipeline contributor",
                        Exception = e
                    });

                    nextStep = PipelineContinuation.Abort;
                }

                return nextStep;
            }
        }

        protected virtual void FinishPipeline(ICommunicationContext context)
        {
            this.PipelineLog.WriteInfo("Pipeline finished.");
        }


        private IEnumerable<ContributorCall> GenerateCallGraph()
        {
            var bootstrapper = this.contributors.OfType<KnownStages.IBegin>().Single();
            var tree = new DependencyTree<ContributorNotification>(
                new ContributorNotification(bootstrapper, new Notification(this, null)));

            foreach (var contrib in this.contributors.Where(x => x != bootstrapper))
            {
                this.notificationRegistrations.Clear();
                
                using (this.PipelineLog.Operation(this, "Initializing contributor {0}.".With(contrib.GetType().Name)))
                {
                    contrib.Initialize(this);
                }

                foreach (var reg in this.notificationRegistrations.DefaultIfEmpty(new Notification(this, null)))
                {
                    tree.CreateNode(new ContributorNotification(contrib, reg));
                }
            }
            foreach (var notificationNode in tree.Nodes)
            {
                foreach (var parentNode in GetCompatibleTypes(tree, notificationNode, notificationNode.Value.Notification.AfterTypes))
                {
                    parentNode.ChildNodes.Add(notificationNode);
                }

                foreach (var childNode in GetCompatibleTypes(tree, notificationNode, notificationNode.Value.Notification.BeforeTypes))
                {
                    childNode.ParentNodes.Add(notificationNode);
                }
            }

            var graph = tree.GetCallGraph().Select(x => new ContributorCall(x.Value.Contributor, x.Value.Notification.Target, x.Value.Notification.Description));
            
            this.LogContributorCallChainCreated(graph);
            
            return graph;
        }

        private static IEnumerable<DependencyNode<ContributorNotification>> GetCompatibleTypes(DependencyTree<ContributorNotification> tree,
                                                                                DependencyNode<ContributorNotification> notificationNode,
                                                                                IEnumerable<Type> beforeTypes)
        {
            return from childType in beforeTypes
                   from compatibleNode in tree.Nodes
                   where compatibleNode != notificationNode
                         && childType.IsAssignableFrom(compatibleNode.Value.Contributor.GetType())
                   select compatibleNode;
        }

        private IEnumerable<IPipelineContributor> GetContributorsOfType(Type contributorType)
        {
            return from contributor in this.contributors
                   where contributorType.IsAssignableFrom(contributor.GetType())
                   select contributor;
        }

        private void LogContributorCallChainCreated(IEnumerable<ContributorCall> callGraph)
        {
            this.PipelineLog.WriteInfo("Contributor call chain has been processed and results in the following pipeline:");
            
            int pos = 0;

            foreach (var contributor in callGraph)
            {
                this.PipelineLog.WriteInfo("{0} {1}", pos++, contributor.ContributorTypeName);
            }
        }

        private void VerifyContributorIsRegistered(Type contributorType)
        {
            if (!this.GetContributorsOfType(contributorType).Any())
            {
                throw new ArgumentOutOfRangeException("There is no registered contributor matching type " + contributorType.FullName);
            }
        }

        private struct ContributorNotification
        {
            public readonly IPipelineContributor Contributor;
            public readonly Notification Notification;

            public ContributorNotification(IPipelineContributor contributor, Notification notification)
            {
                this.Notification = notification;
                this.Contributor = contributor;
            }
        }

        private class Notification : IPipelineExecutionOrder, IPipelineExecutionOrderAnd
        {
            private readonly ICollection<Type> after = new List<Type>();
            private readonly ICollection<Type> before = new List<Type>();
            private readonly PipelineRunner runner;

            public Notification(PipelineRunner runner, Func<ICommunicationContext, PipelineContinuation> action)
            {
                this.runner = runner;
                this.Target = action;
            }

            public ICollection<Type> AfterTypes
            {
                get { return this.after; }
            }

            public IPipelineExecutionOrder And
            {
                get { return this; }
            }

            public ICollection<Type> BeforeTypes
            {
                get { return this.before; }
            }

            public string Description
            {
                get { return this.Target != null && this.Target.Target != null ? this.Target.Target.GetType().Name : null; }
            }

            public Func<ICommunicationContext, PipelineContinuation> Target { get; private set; }

            public IPipelineExecutionOrderAnd After(Type contributorType)
            {
                this.runner.VerifyContributorIsRegistered(contributorType);
                this.AfterTypes.Add(contributorType);

                return this;
            }

            public IPipelineExecutionOrderAnd Before(Type contributorType)
            {
                this.runner.VerifyContributorIsRegistered(contributorType);
                this.BeforeTypes.Add(contributorType);
                
                return this;
            }
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