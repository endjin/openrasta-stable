namespace OpenRasta.Testing.Hosting
{
    using OpenRasta.Authentication;
    using OpenRasta.Contracts.Authentication;

    public class StaticAuthenticationProvider : IAuthenticationProvider
    {
        public Credentials GetByUsername(string p)
        {
            return new Credentials { Username = "username", Password = "password" };
        }
    }
}