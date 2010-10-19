namespace OpenRasta.Contracts.Authentication
{
    using OpenRasta.Authentication;

    public interface IAuthenticationProvider
    {
        Credentials GetByUsername(string p);
    }
}