using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using YouTrack.REST.API.Helpers;
using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;
using YouTrack.REST.API.Wrappers;

namespace YouTrack.REST.API.Clients.Standard
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Projects-Related-Methods.html">YouTrack Projects Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class ProjectsClient : ApiClient, IProjectsClient
    {
        public ProjectsClient(IEnterpriseYouTrackRestClient restClient, Connection connection) : base(restClient,
            connection)
        {

        }

        /// <summary>
        /// Get a list of all accessible projects from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Accessible-Projects.html">Get Accessible Projects</a>.</remarks>
        /// <param name="verbose">If full representation of projects is returned. If this parameter is false, only short names and id's are returned.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Project" /> that are accessible for currently logged in user.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<Project>> GetAccessibleProjects(bool verbose = false)
        {

            var url = ProjectsUrls.AccessibleProjects(verbose);

            var request = new YouTrackRestRequest(url, Method.GET);

            var response = await RestClient.ExecuteTaskAsync<List<Project>>(request);

            return response.Data;
        }
    }
}