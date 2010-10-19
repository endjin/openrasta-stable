namespace OpenRasta.Authentication.Digest
{
    #region Using Directives

    using OpenRasta.Contracts.Authentication;
    using OpenRasta.Contracts.Authentication.Digest;
    using OpenRasta.Contracts.Web;

    #endregion

    public class DigestAuthenticationScheme : IAuthenticationScheme
    {
        private readonly IDigestAuthenticator digestAuthenticator;

        public DigestAuthenticationScheme(IDigestAuthenticator digestAuthenticator)
        {
            this.digestAuthenticator = digestAuthenticator;
        }

        public string Name
        {
            get { return "Basic"; }
        }

        public AuthenticationResult Authenticate(IRequest request)
        {
            DigestAuthRequestParameters credentials;

            if (DigestAuthRequestParameters.TryParse(request.Headers["Authorization"], out credentials))
            {
                return this.digestAuthenticator.Authenticate(credentials);
            }

            return new AuthenticationResult.MalformedCredentials();
        }

        public void Challenge(IResponse response)
        {
            response.Headers["WWW-Authenticate"] = string.Format("Basic realm=\"{0}\"", this.digestAuthenticator.Realm);
        }
    }
}