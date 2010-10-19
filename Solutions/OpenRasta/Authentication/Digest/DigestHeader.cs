namespace OpenRasta.Authentication.Digest
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;

    #endregion

    public class DigestHeader
    {
        private readonly Dictionary<string, string> values = new Dictionary<string, string>();

        public DigestHeader()
        {
        }

        public DigestHeader(DigestHeader copy)
        {
            foreach (var kv in copy.values)
            {
                this.values.Add(kv.Key, kv.Value);
            }
        }

        public string ClientNonce
        {
            get { return this.values.ContainsKey("cnonce") ? this.values["cnonce"] : null; }
            set { this.values["cnonce"] = value; }
        }

        public string ClientRequestHeader
        {
            get
            {
                var builder = new StringBuilder();

                builder.AppendFormat("Digest username=\"{0}\", realm=\"{1}\",nonce=\"{2}\"", this.Username, this.Realm, this.Nonce);
                builder.AppendFormat(
                    @",uri=""{0}"",qop={1},nc={2},cnonce=""{3}"",response=""{4}"", opaque=""{5}""",
                    this.Uri,
                    this.QualityOfProtection,
                    this.NonceCount,
                    this.ClientNonce,
                    this.Response,
                    this.Opaque);

                return builder.ToString();
            }
        }

        public string Nonce
        {
            get { return this.values.ContainsKey("nonce") ? this.values["nonce"] : null; }
            set { this.values["nonce"] = value; }
        }

        public string NonceCount
        {
            get { return this.values.ContainsKey("nc") ? this.values["nc"] : null; }
            set { this.values["nc"] = value; }
        }

        public string Opaque
        {
            get { return this.values.ContainsKey("opaque") ? this.values["opaque"] : null; }
            set { this.values["opaque"] = value; }
        }

        public string Password { get; set; }

        public string QualityOfProtection
        {
            get { return this.values.ContainsKey("qop") ? this.values["qop"] : null; }
            set { this.values["qop"] = value; }
        }

        public string Realm
        {
            get { return this.values.ContainsKey("realm") ? this.values["realm"] : null; }
            set { this.values["realm"] = value; }
        }

        public string Response
        {
            get { return this.values.ContainsKey("response") ? this.values["response"] : null; }
            set { this.values["response"] = value; }
        }

        public string ServerResponseHeader
        {
            get
            {
                var builder = new StringBuilder();

                builder.AppendFormat("Digest realm=\"{0}\",nonce=\"{1}\",opaque=\"{2}\"", this.Realm, this.Nonce, this.Opaque);
                builder.AppendFormat(",stale={0}", this.Stale);
                builder.Append(",algorithm=MD5, qop=\"auth\"");
                
                return builder.ToString();
            }
        }

        public bool Stale
        {
            get
            {
                string stale;
                return this.values.TryGetValue("stale", out stale) ? bool.Parse(stale) : false;
            }

            set
            {
                this.values["stale"] = value ? "TRUE" : "FALSE";
            }
        }

        public string Uri
        {
            get { return this.values.ContainsKey("uri") ? this.values["uri"] : null; }
            set { this.values["uri"] = value; }
        }

        public string Username
        {
            get { return this.values.ContainsKey("username") ? this.values["username"] : null; }
            set { this.values["username"] = value; }
        }

        public static DigestHeader Parse(string header)
        {
            if (!header.ToUpper().StartsWith("DIGEST"))
            {
                return null;
            }

            var credentials = new DigestHeader();
            string arguments = header.Substring(6);

            string[] keyValues = arguments.Split(',');
            
            foreach (string kv in keyValues)
            {
                string[] parts = kv.Split(new[] { '=' }, 2);
                string key = parts[0].Trim(' ', '\t', '\r', '\n', '\"');
                string value = parts[1].Trim(' ', '\t', '\r', '\n', '\"');
                credentials.values.Add(key, value);
            }
            
            return credentials;
        }

        public string GetCalculatedResponse(string httpMethod)
        {
            // A1 = unq(username-value) ":" unq(realm-value) ":" passwd
            string A1 = String.Format("{0}:{1}:{2}", this.Username, this.Realm, this.Password);

            // H(A1) = MD5(A1)
            string HA1 = GetMD5HashBinHex(A1);

            // A2 = Method ":" digest-uri-value
            string A2 = String.Format("{0}:{1}", httpMethod, this.Uri);

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
                    this.Nonce,
                    this.NonceCount,
                    this.ClientNonce,
                    this.QualityOfProtection,
                    HA2);
            }
            else
            {
                unhashedDigest = String.Format(
                    "{0}:{1}:{2}",
                    HA1,
                    this.NonceCount,
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