namespace YouTrack.REST.API.Authentication
{
    public class Credentials
    {
        public Credentials()
        {
            AuthenticationType = AuthenticationType.Anonymous;
        }

        public Credentials(string token)
        {
            Login = null;
            Password = token;
            AuthenticationType = AuthenticationType.OAuth;
        }

        public Credentials(string login, string password)
        {
            Login = login;
            Password = password;
            AuthenticationType = AuthenticationType.Simple;
        }

        public string Login { get; private set; }

        public string Password { get; private set; }

        public AuthenticationType AuthenticationType { get; private set; }
    }
}
