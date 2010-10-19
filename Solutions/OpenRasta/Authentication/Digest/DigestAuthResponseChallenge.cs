namespace OpenRasta.Authentication.Digest
{
    #region Using Directives

    using System;
    using System.Security.Cryptography;
    using System.Text;

    #endregion

    public class DigestAuthResponseChallenge
    {
        public DigestAuthResponseChallenge(string realm, string serverNonce, byte[] opaqueData, bool stale)
        {
            this.Realm = realm;
            this.ServerNonce = serverNonce;
            this.Stale = stale;
        }

        public DigestAlgorithm Algorithm { get; private set; }

        public string ClientNonce { get; private set; }

        public string Digest { get; private set; }

        public string Opaque { get; private set; }

        public string Password { get; private set; }

        public string QualityOfProtection { get; private set; }

        public string Realm { get; private set; }

        public string RequestCounter { get; private set; }

        public string Response { get; private set; }

        public string Salt { get; private set; }

        public string ServerNonce { get; private set; }

        public bool Stale { get; private set; }

        public string Uri { get; private set; }

        public string Username { get; private set; }

        public string GetCalculatedResponse(string httpMethod)
        {
            // A1 = unq(username-value) ":" unq(realm-value) ":" passwd
            string A1 = String.Format("{0}:{1}:{2}", Username, Realm, Password);

            // H(A1) = MD5(A1)
            string HA1 = GetMD5HashBinHex(A1);

            // A2 = Method ":" digest-uri-value
            string A2 = String.Format("{0}:{1}", httpMethod, Uri);

            // H(A2)
            string HA2 = GetMD5HashBinHex(A2);

            // KD(secret, data) = H(concat(secret, ":", data))
            // if qop == auth:
            // request-digest  = <"> < KD ( H(A1),     unq(nonce-value)
            // ":" nc-value
            // ":" unq(cnonce-value)
            // ":" unq(qop-value)
            // ":" H(A2)
            // ) <">
            // if qop is missing,
            // request-digest  = <"> < KD ( H(A1), unq(nonce-value) ":" H(A2) ) > <">
            string unhashedDigest;

            if (this.QualityOfProtection != null)
            {
                unhashedDigest = String.Format(
                    "{0}:{1}:{2}:{3}:{4}:{5}",
                    HA1,
                    this.ServerNonce,
                    this.RequestCounter,
                    this.ClientNonce,
                    this.QualityOfProtection,
                    HA2);
            }
            else
            {
                unhashedDigest = String.Format(
                    "{0}:{1}:{2}",
                    HA1,
                    this.RequestCounter,
                    HA2);
            }

            return GetMD5HashBinHex(unhashedDigest);
        }

        private static string GetMD5HashBinHex(string value)
        {
            MD5 hash = MD5.Create();
            byte[] result = hash.ComputeHash(Encoding.ASCII.GetBytes(value));

            var sb = new StringBuilder();
            
            foreach (byte b in result)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}