using System;
using System.Net;
using YouTrack.REST.API.Exceptions;

namespace YouTrackClientVS.Infrastructure.Utils
{
    public static class ExceptionMapper
    {
        public static string Map(Exception ex)
        {
            if (ex is GitClientVsException gitEx)
                return gitEx.Message;
            if (ex is WebException webExc)
                return webExc.Message;
            if (ex is AuthorizationException)
                return "Incorrect credentials";
            if (ex is ForbiddenException)
                return "Operation is forbidden";
            if (ex is RequestFailedException reqFailedEx)
                return reqFailedEx.IsFriendlyMessage ? ex.Message : "Wrong request";

            if (ex is UnauthorizedAccessException)
                return "Unauthorized";

            return $"Unknown error. ({ex.Message}). Check logs for more info";
        }
    }
}
