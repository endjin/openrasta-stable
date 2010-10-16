namespace OpenRasta.Authentication.Basic
{
    public class BasicAuthRequestHeader
    {
        internal BasicAuthRequestHeader(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Password { get; private set; }

        public string Username { get; private set; }
    }
}