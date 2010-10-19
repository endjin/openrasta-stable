namespace OpenRasta.Contracts.Authentication.Basic
{
    using OpenRasta.Authentication;
    using OpenRasta.Authentication.Basic;

    public interface IBasicAuthenticator
    {
        string Realm { get; }

        AuthenticationResult Authenticate(BasicAuthRequestHeader header);
    }
}