namespace OpenRasta.Authorization
{
    #region Using Directives

    using System;
    using System.Linq;

    using OpenRasta.Authentication;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Pipeline;
    using OpenRasta.Web;

    #endregion

    public class PrincipalAuthorizationAttribute : RequiresAuthenticationAttribute
    {
        public string[] InRoles { get; set; }

        public string[] Users { get; set; }

        public PipelineContinuation ExecuteBefore(ICommunicationContext context)
        {
            if ((this.InRoles == null || this.InRoles.Length == 0) && (this.Users == null || this.Users.Length == 0))
            {
                return PipelineContinuation.Continue;
            }

            if (this.ExecuteBefore(context) == PipelineContinuation.Continue)
            {
                if (this.InRoles != null)
                {
                    if (this.InRoles.Any(role => context.User.IsInRole(role)))
                    {
                        return PipelineContinuation.Continue;
                    }
                }

                if (this.Users != null)
                {
                    if (this.Users.Any(user => context.User.Identity.Name == user))
                    {
                        return PipelineContinuation.Continue;
                    }
                }
            }

            context.OperationResult = new OperationResult.Unauthorized();
            
            return PipelineContinuation.RenderNow;
        }
    }
}