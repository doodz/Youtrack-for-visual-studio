using System.Collections.Generic;
using RestSharp.Authenticators;

namespace YouTrack.REST.API.Authentication
{
    class Authenticator
    {
        public Authenticator(Credentials credentials)
        {
            Credentials = credentials;
            Authenticators = new Dictionary<AuthenticationType, IAuthenticator>
            {
                { AuthenticationType.Anonymous, new AnonymousAuthenticator() },
                { AuthenticationType.Basic, new HttpBasicAuthenticator(credentials.Login, credentials.Password) },
                { AuthenticationType.Simple, new SimpleAuthenticator("login", credentials.Login, "password", credentials.Password)},
            };



            CreatedAuthenticator = Authenticators[credentials.AuthenticationType];
        }

        public IAuthenticator CreatedAuthenticator { get; private set; }

        public Credentials Credentials { get; private set; }

        private Dictionary<AuthenticationType, IAuthenticator> Authenticators { get; set; }

    }
}