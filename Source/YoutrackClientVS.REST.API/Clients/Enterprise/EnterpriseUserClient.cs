using System;
using System.Threading.Tasks;
using RestSharp;
using YouTrack.REST.API.Exceptions;
using YouTrack.REST.API.Helpers;
using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;
using YouTrack.REST.API.Wrappers;

namespace YouTrack.REST.API.Clients.Enterprise
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/User-Related-Methods.html">YouTrack User Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class EnterpriseUserClient : ApiClient, IUserClient
    {
        public EnterpriseUserClient(IEnterpriseYouTrackRestClient restClient, Connection connection) : base(restClient,
            connection)
        {
        }

        /// <summary>
        /// Get info about currently logged in user.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Info-For-Current-User.html">Get Info For Current User</a>.</remarks>
        /// <returns>A <see cref="User" /> instance that represents the currently logged in user.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<User> GetCurrentUserInfo()
        {
            var url = UserUrls.CurrentUserInfo();

            var request = new YouTrackRestRequest(url, Method.GET);

            var response = await RestClient.ExecuteTaskAsync<User>(request);

            return response.Data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <a href="https://www.jetbrains.com/help/youtrack/standalone/Log-in-to-YouTrack.html">Login to YouTrack</a>
        ///  </remarks>
        /// <returns></returns>
        public async Task<User> Login()
        {
            var url = UserUrls.Login();
            var request = new YouTrackRestRequest(url, Method.POST);
            request.AddParameter("login", Connection.Credentials.Login);
            request.AddParameter("password", Connection.Credentials.Password);

            var response = await RestClient.ExecuteTaskAsync(request);

            if (!string.Equals(response.Content, "<login>ok</login>", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedConnectionException("Could not authenticate. Server did not return expected authentication response. Check the Response property for more details.", response.StatusCode, response.Content);

            return await GetCurrentUserInfo();
        }
    }
}