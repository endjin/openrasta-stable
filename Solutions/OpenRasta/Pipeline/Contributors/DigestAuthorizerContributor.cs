// Digest Authentication implementation
//  Inspired by mono's implenetation, rewritten for OpenRasta.
// Original authors:
//  Greg Reinacker (gregr@rassoc.com)
//  Sebastien Pouliot (spouliot@motus.com)
// Portions (C) 2002-2003 Greg Reinacker, Reinacker & Associates, Inc. All rights reserved.
// Portions (C) 2003 Motus Technologies Inc. (http://www.motus.com)
// Original source code available at
// http://www.rassoc.com/gregr/weblog/stories/2002/07/09/webServicesSecurityHttpDigestAuthenticationWithoutActiveDirectory.html

namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Security.Principal;

    using OpenRasta.Authentication;
    using OpenRasta.Authentication.Digest;
    using OpenRasta.Contracts.Authentication;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;
    using OpenRasta.Pipeline;
    using OpenRasta.Web;

    #endregion

    public class DigestAuthorizerContributor : IPipelineContributor
    {
        private readonly IDependencyResolver resolver;
        private IAuthenticationProvider authentication;

        public DigestAuthorizerContributor(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.ReadCredentials)
                .After<KnownStages.IBegin>()
                .And
                .Before<KnownStages.IHandlerSelection>();

            pipelineRunner.Notify(WriteCredentialRequest)
                .After<KnownStages.IOperationResultInvocation>()
                .And
                .Before<KnownStages.IResponseCoding>();
        }

        public PipelineContinuation ReadCredentials(ICommunicationContext context)
        {
            if (!this.resolver.HasDependency(typeof(IAuthenticationProvider)))
            {
                return PipelineContinuation.Continue;
            }

            this.authentication = this.resolver.Resolve<IAuthenticationProvider>();

            DigestHeader authorizeHeader = GetDigestHeader(context);

            if (authorizeHeader == null)
            {
                return PipelineContinuation.Continue;
            }

            string digestUri = GetAbsolutePath(authorizeHeader.Uri);

            if (digestUri != context.Request.Uri.AbsolutePath)
            {
                return ClientError(context);
            }

            Credentials creds = this.authentication.GetByUsername(authorizeHeader.Username);

            if (creds == null)
            {
                return NotAuthorized(context);
            }

            var checkHeader = new DigestHeader(authorizeHeader) { Password = creds.Password, Uri = authorizeHeader.Uri };
            string hashedDigest = checkHeader.GetCalculatedResponse(context.Request.HttpMethod);

            if (authorizeHeader.Response == hashedDigest)
            {
                IIdentity id = new GenericIdentity(creds.Username, "Digest");
                context.User = new GenericPrincipal(id, creds.Roles);
                
                return PipelineContinuation.Continue;
            }

            return NotAuthorized(context);
        }

        private static DigestHeader GetDigestHeader(ICommunicationContext context)
        {
            string header = context.Request.Headers["Authorization"];
            
            return string.IsNullOrEmpty(header) ? null : DigestHeader.Parse(header);
        }

        private static bool HasDigestHeader(ICommunicationContext context)
        {
            return GetDigestHeader(context) != null;
        }
        
        private static PipelineContinuation ClientError(ICommunicationContext context)
        {
            context.OperationResult = new OperationResult.BadRequest();
            
            return PipelineContinuation.RenderNow;
        }

        private static string GetAbsolutePath(string uri)
        {
            uri = uri.TrimStart();

            if (uri.StartsWith("http://") || uri.StartsWith("https://"))
            {
                return new Uri(uri).AbsolutePath;
            }

            return uri.Any(ch => ch > 127) ? Uri.EscapeUriString(uri) : uri;
        }

        private static PipelineContinuation NotAuthorized(ICommunicationContext context)
        {
            context.OperationResult = new OperationResult.Unauthorized();
            
            return PipelineContinuation.RenderNow;
        }

        private static PipelineContinuation WriteCredentialRequest(ICommunicationContext context)
        {
            if (context.OperationResult is OperationResult.Unauthorized)
            {
                context.Response.Headers["WWW-Authenticate"] =
                    new DigestHeader
                        {
                            Realm = "Digest Authentication",
                            QualityOfProtection = "auth",
                            Nonce = "nonce",
                            Stale = false,
                            Opaque = "opaque"
                        }.ServerResponseHeader;
            }

            return PipelineContinuation.Continue;
        }
    }
}