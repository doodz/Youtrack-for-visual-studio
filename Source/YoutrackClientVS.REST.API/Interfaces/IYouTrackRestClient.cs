using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using YouTrack.REST.API.QueryBuilders;

namespace YouTrack.REST.API.Interfaces
{
    public interface IYouTrackRestClient
    {
        Task<IEnumerable<T>> GetAllPages<T>(string url, int limit = 50, QueryString query = null);
        Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request);
        Task<IRestResponse> ExecuteTaskAsync(IRestRequest request);
    }
}