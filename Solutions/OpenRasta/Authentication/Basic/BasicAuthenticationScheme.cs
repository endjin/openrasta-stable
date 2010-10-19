namespace OpenRasta.Authentication.Basic
{
    #region Using Directives

    using OpenRasta.Contracts.Authentication;
    using OpenRasta.Contracts.Authentication.Basic;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Extensions;

    #endregion

    public class BasicAuthenticationScheme : IAuthenticationScheme
    {
        private const string Scheme = "Basic";

        private readonly IBasicAuthenticator basicAuthenticator;

        public BasicAuthenticationScheme(IBasicAuthenticator basicAuthenticator)
        {
            this.basicAuthenticator = basicAuthenticator;
        }

        public string Name
        {
            get { return Scheme; }
        }

        public AuthenticationResult Authenticate(IRequest request)
        {
            BasicAuthRequestHeader credentials = ExtractBasicHeader(request.Headers["Authorization"]);

            if (credentials != null)
            {
                return this.basicAuthenticator.Authenticate(credentials);
            }

            return new AuthenticationResult.MalformedCredentials();
        }

        public void Challenge(IResponse response)
        {
            response.Headers["WWW-Authenticate"] = string.Format("{0} realm=\"{1}\"", Scheme, this.basicAuthenticator.Realm);
        }

        private static BasicAuthRequestHeader ExtractBasicHeader(string value)
        {
            try
            {
                var basicBase64Credentials = value.Split(' ')[1];

                var basicCredentials = basicBase64Credentials.FromBase64String().Split(':');

                if (basicCredentials.Length != 2)
                {
                    return null;
                }

                return new BasicAuthRequestHeader(basicCredentials[0], basicCredentials[1]);
            }
            catch
            {
                return null;
            }
        }
    }
}