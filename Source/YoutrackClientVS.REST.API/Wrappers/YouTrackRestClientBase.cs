using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using log4net;
using RestSharp;
using YouTrack.REST.API.Authentication;
using YouTrack.REST.API.Exceptions;
using YouTrack.REST.API.Models.Standard;
using YouTrack.REST.API.QueryBuilders;
using YouTrack.REST.API.Serializers;
using Parameter = RestSharp.Parameter;

namespace YouTrack.REST.API.Wrappers
{
    public abstract class YouTrackRestClientBase : RestClient
    {
        private readonly ILog _logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly Connection Connection;


        protected YouTrackRestClientBase(Connection connection) : base(connection.ApiUrl)
        {
            Connection = connection;

            var serializer = new NewtonsoftJsonSerializer();
            AddHandler("application/json", serializer);
            AddHandler("text/json", serializer);
            AddHandler("text/plain", serializer);
            AddHandler("text/x-json", serializer);
            AddHandler("text/javascript", serializer);
            AddHandler("*+json", serializer);
           
            //var auth = new Authenticator(connection.Credentials);
            //Authenticator = auth.CreatedAuthenticator;
            FollowRedirects = false;
            CookieContainer = new CookieContainer();
        }


        public abstract Task<IEnumerable<T>> GetAllPages<T>(string url, int limit = 50, QueryString query = null);
        protected abstract string DeserializeError(IRestResponse response);

        public override async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            _logger.Info($"Calling ExecuteTaskAsync. BaseUrl: {BaseUrl} Resource: {request.Resource} Parameters: {string.Join(", ", request.Parameters)}");
            AddHeaders(request);
            var response = await base.ExecuteTaskAsync<T>(request);
            response = await RedirectIfNeededAndGetResponse(response, request);
            CheckResponseStatusCode(response);
            return response;
        }

        public override async Task<IRestResponse> ExecuteTaskAsync(IRestRequest request)
        {
            _logger.Info($"Calling ExecuteTaskAsync. BaseUrl: {BaseUrl} Resource: {request.Resource} Parameters: {string.Join(", ", request.Parameters)}");
            AddHeaders(request);
            var response = await base.ExecuteTaskAsync(request);
            response = await RedirectIfNeededAndGetResponse(response, request);
            CheckResponseStatusCode(response);
            return response;
        }


        private void AddHeaders(IRestRequest request)
        {
            //request.AddHeader("Accept");
            request.AddHeader("Accept", "application/json");
        }

        private void CheckResponseStatusCode(IRestResponse response)
        {
            if (response.ErrorException != null)
                throw response.ErrorException;

            if (response.StatusCode == HttpStatusCode.Unauthorized) throw new AuthorizationException();

            if (response.StatusCode == HttpStatusCode.Forbidden) throw new ForbiddenException(response.ErrorMessage);

            if ((int)response.StatusCode >= 400)
            {
                var errorMessage = response.ErrorMessage;
                var friendly = false;
                if (response.Content != null)
                    try
                    {
                        errorMessage = DeserializeError(response);
                        friendly = true;
                    }
                    catch (Exception)
                    {
                    }

                _logger.Error($"Error in request: {response.Content}");

                throw new RequestFailedException(errorMessage, friendly);
            }
        }


        private async Task<IRestResponse<T>> RedirectIfNeededAndGetResponse<T>(IRestResponse<T> response,
            IRestRequest request)
        {
            while (response.StatusCode == HttpStatusCode.Redirect)
            {
                var newLocation = GetNewLocationFromHeader(response);

                if (newLocation == null)
                    return response;

                request.Resource = RemoveBaseUrl(newLocation);
                response = await base.ExecuteTaskAsync<T>(request);
            }

            return response;
        }

        private async Task<IRestResponse> RedirectIfNeededAndGetResponse(IRestResponse response, IRestRequest request)
        {
            while (response.StatusCode == HttpStatusCode.Redirect)
            {
                var newLocation = GetNewLocationFromHeader(response);

                if (newLocation == null)
                    return response;

                request.Resource = RemoveBaseUrl(newLocation);
                response = await base.ExecuteTaskAsync(request);
            }

            return response;
        }


        private static Parameter GetNewLocationFromHeader(IRestResponse response)
        {
            return response.Headers
                .FirstOrDefault(x => x.Type == ParameterType.HttpHeader &&
                                     x.Name.Equals(HttpResponseHeader.Location.ToString(),
                                         StringComparison.InvariantCultureIgnoreCase));
        }

        private string RemoveBaseUrl(Parameter newLocation)
        {
            return newLocation.Value.ToString().Replace(Connection.ApiUrl.ToString(), string.Empty);
        }
    }
}