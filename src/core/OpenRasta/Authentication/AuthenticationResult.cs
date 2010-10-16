namespace OpenRasta.Authentication
{
    public class AuthenticationResult
    {
        public class MalformedCredentials : AuthenticationResult
        {
        }

        public class Failed : AuthenticationResult
        {
        }

        public class Success : AuthenticationResult
        {
            public Success(string username, params string[] roles)
            {
                this.Username = username;
                this.Roles = roles;
            }

            public string[] Roles { get; private set; }

            public string Username { get; private set; }
        }
    }
}