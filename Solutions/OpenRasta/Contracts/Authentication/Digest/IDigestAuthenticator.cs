namespace OpenRasta.Contracts.Authentication.Digest
{
    using OpenRasta.Authentication;
    using OpenRasta.Authentication.Digest;

    public interface IDigestAuthenticator
    {
        string Realm { get; }
        AuthenticationResult Authenticate(DigestAuthRequestParameters header);
    }
}