using System;
using System.Threading.Tasks;
using YouTrack.REST.API.Authentication;

namespace YouTrack.REST.API.Interfaces
{
    public interface IBitbucketClientFactory
    {
        Task<IYouTrackClient> CreateEnterpriseBitBucketClient(Uri host, Credentials cred);
        Task<IYouTrackClient> CreateStandardBitBucketClient(Credentials cred);
    }
}