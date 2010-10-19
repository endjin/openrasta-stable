namespace OpenRasta.Testing.Hosting
{
    using OpenRasta.Security;

    public class StaticAuthenticationProvider : IAuthenticationProvider
    {
        public Credentials GetByUsername(string p)
        {
            return new Credentials { Username = "username", Password = "password" };
        }
    }
}