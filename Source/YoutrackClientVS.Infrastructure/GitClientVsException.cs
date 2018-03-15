using System;

namespace YouTrackClientVS.Infrastructure
{
    public class GitClientVsException : Exception
    {
        public GitClientVsException(string message) : base(message) { }
    }
}
