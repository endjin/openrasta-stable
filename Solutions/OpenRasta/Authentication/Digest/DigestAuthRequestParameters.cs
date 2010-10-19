namespace OpenRasta.Authentication.Digest
{
    #region Using Directives

    using System;
    using System.Text;

    using OpenRasta.Extensions;

    #endregion

    public class DigestAuthRequestParameters
    {
        private const string SchemeName = "DIGEST";
        private const string SchemeNameWithSpace = SchemeName + " ";

        public DigestAuthRequestParameters(string username)
        {
            this.Username = username;
        }

        public string ClientNonce { get; private set; }

        public string Digest { get; private set; }

        public string QualityOfProtection { get; private set; } 
        
        public string ServerNonce { get; private set; }

        public string Opaque { get; private set; }

        public string Realm { get; private set; }

        public string RequestCounter { get; private set; }

        public string Response { get; private set; }

        public string Uri { get; private set; }

        public string Username { get; private set; }

        public static DigestAuthRequestParameters Parse(string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return null;
            }

            if (!value.ToUpper().StartsWith(SchemeNameWithSpace))
            {
                return null;
            }

            var basicBase64Credentials = value.Split(' ')[1];
            var basicCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(basicBase64Credentials)).Split(':');

            var username = basicCredentials[0];
            var password = basicCredentials[1];

            return new DigestAuthRequestParameters(username);
        }

        public static bool TryParse(string value, out DigestAuthRequestParameters credentials)
        {
            credentials = null;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (!value.ToUpper().StartsWith(SchemeNameWithSpace))
            {
                return false;
            }

            var basicBase64Credentials = value.Split(' ')[1];

            credentials = ExtractDigestCredentials(basicBase64Credentials);

            return true;
        }

        private static DigestAuthRequestParameters ExtractDigestCredentials(string basicCredentialsAsBase64)
        {
            var basicCredentials = basicCredentialsAsBase64.FromBase64String().Split(':');

            var username = basicCredentials[0];
            var password = basicCredentials[1];

            return new DigestAuthRequestParameters(username);
        }
    }
}