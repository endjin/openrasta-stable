namespace OpenRasta.Web.Filters
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Pipeline;

    #endregion

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FilterAttribute : Attribute
    {
        public virtual PipelineContinuation ExecuteBefore(ICommunicationContext context)
        {
            return PipelineContinuation.Continue;
        }

        public virtual PipelineContinuation ExecuteAfter(ICommunicationContext context)
        {
            return PipelineContinuation.Continue;
        }
    }
}