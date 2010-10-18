namespace OpenRasta.Authentication
{
    using OpenRasta.Web;

    public interface IAuthenticationScheme
    {
        string Name { get; }

        AuthenticationResult Authenticate(IRequest request);

        void Challenge(IResponse response);
    }
}