using System.Collections.Generic;
using System.Threading.Tasks;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Interfaces
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Projects-Related-Methods.html">YouTrack Projects Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public interface IProjectsClient
    {
        /// <summary>
        /// Get a list of all accessible projects from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Accessible-Projects.html">Get Accessible Projects</a>.</remarks>
        /// <param name="verbose">If full representation of projects is returned. If this parameter is false, only short names and id's are returned.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Project" /> that are accessible for currently logged in user.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<ICollection<Project>> GetAccessibleProjects(bool verbose = false);
    }
}