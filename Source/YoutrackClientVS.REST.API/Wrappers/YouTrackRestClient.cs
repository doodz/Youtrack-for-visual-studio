using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;
using YouTrack.REST.API.QueryBuilders;

namespace YouTrack.REST.API.Wrappers
{
    public class YouTrackRestClient : YouTrackRestClientBase, IYouTrackRestClient
    {
        public YouTrackRestClient(Connection connection) : base(connection)
        {
        }

        public override async Task<IEnumerable<T>> GetAllPages<T>(string url, int limit = 50, QueryString query = null)
        {
            var result = new IteratorBasedPage<T>()
            {
                Values = new List<T>()
            };
            IRestResponse<IteratorBasedPage<T>> response;
            var pageNumber = 1;
            do
            {
                var request = new YouTrackRestRequest(url, Method.GET);
                request.AddQueryParameter("pagelen", limit.ToString()).AddQueryParameter("page", pageNumber.ToString());

                if (query != null)
                    foreach (var par in query)
                        request.AddQueryParameter(par.Key, par.Value);

                response = await ExecuteTaskAsync<IteratorBasedPage<T>>(request);

                if (response.Data?.Values == null)
                    break;

                result.Values.AddRange(response.Data.Values);

                pageNumber++;
            } while (response.Data?.Next != null); //todo 99% this value should be used instead of pagenumber

            return result.Values;
        }

        protected override string DeserializeError(IRestResponse response)
        {
            var errorObject = JObject.Parse(response.Content);

            var error = (JObject)errorObject["error"];
            var message = error["message"].Value<string>();
            var fields = error["fields"];

            var fieldsMessage = string.Empty;

            if (fields != null)
            {
                fieldsMessage = string.Join(", ", fields.SelectMany(x => ((JProperty)x).Value.Values<string>()));
                message += ". " + fieldsMessage;
            }

            return message;
        }
    }
}