namespace OpenRasta.Contracts.Authentication
{
    using OpenRasta.Authentication;
    using OpenRasta.Contracts.Web;

    public interface IAuthenticationScheme
    {
        string Name { get; }

        AuthenticationResult Authenticate(IRequest request);

        void Challenge(IResponse response);
    }
}