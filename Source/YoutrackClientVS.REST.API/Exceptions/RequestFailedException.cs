using System;

namespace YouTrack.REST.API.Exceptions
{
    public class RequestFailedException : Exception
    {
        public bool IsFriendlyMessage { get; set; }

        public RequestFailedException(string message, bool isFriendlyMessage) : base(message)
        {
            IsFriendlyMessage = isFriendlyMessage;
        }


    }
}