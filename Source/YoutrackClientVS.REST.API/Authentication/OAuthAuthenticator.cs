namespace YouTrack.REST.API.Authentication
{
    public class OAuthAuthenticator
    {
        private const string TokenUrl = "https://bitbucket.org/site/oauth2/access_token";
        private const string TokenType = "Bearer";

        public OAuthAuthenticator(Credentials credentials)
        {
        }
    }
}