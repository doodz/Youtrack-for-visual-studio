using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace YouTrack.REST.API.Serializers
{
    public interface IJsonSerializer : ISerializer, IDeserializer
    {
        
    }
}