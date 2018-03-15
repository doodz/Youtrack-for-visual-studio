using System.Threading.Tasks;
using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Interfaces
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/User-Related-Methods.html">YouTrack User Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public interface IUserClient
    {
        /// <summary>
        /// Get info about currently logged in user.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Info-For-Current-User.html">Get Info For Current User</a>.</remarks>
        /// <returns>A <see cref="User" /> instance that represents the currently logged in user.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<User> GetCurrentUserInfo();

        Task<User> Login();
    }
}