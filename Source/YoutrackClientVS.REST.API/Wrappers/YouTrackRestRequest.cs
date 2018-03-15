using RestSharp;
using YouTrack.REST.API.Serializers;

namespace YouTrack.REST.API.Wrappers
{
    internal class YouTrackRestRequest : RestRequest
    {
        public YouTrackRestRequest(string resource, Method method) : base(resource, method)
        {
            JsonSerializer = new NewtonsoftJsonSerializer();
        }
    }
}