using System;

namespace YouTrack.REST.API.Exceptions
{
    public class AuthorizationException : Exception
    {
        public override string Message => "Unauthorized";
    }
}