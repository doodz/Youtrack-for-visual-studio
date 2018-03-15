using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;
using YouTrack.REST.API.Helpers;
using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;
using YouTrack.REST.API.Wrappers;

namespace YouTrack.REST.API.Clients.Standard
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Users.html">administering user accounts in YouTrack</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class UserManagementClient : ApiClient, IUserManagementClient
    {
        /// <summary>
        /// Creates an instance of the <see cref="UserManagementService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="System.Net.Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public UserManagementClient(IEnterpriseYouTrackRestClient restClient, Connection connection) : base(restClient,
            connection)
        {
        }

        /// <summary>
        /// Get user by login name.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-User.html">Get user by login name</a>.</remarks>
        /// <returns>A <see cref="User" /> instance that represents the user matching <paramref name="username"/> or <value>null</value> if the user does not exist.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="username"/> is null or empty.</exception>
        public async Task<User> GetUser(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            var url = UsersUrls.GetUser(username);
            var request = new YouTrackRestRequest(url, Method.GET);
            var response = await RestClient.ExecuteTaskAsync<User>(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            return response.Data;

        }

        /// <summary>
        /// Get a list of available registered users, paged per 10. Use the <paramref name="start"/> parameter to get subsequent results.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-Users.html">Get a list of all available registered users</a>.</remarks>
        /// <param name="filter">Search query (part of user login, name or email).</param>
        /// <param name="group">Filter by group (groupID).</param>
        /// <param name="role">Filter by role.</param>
        /// <param name="project">Filter by project (projectID)</param>
        /// <param name="permission">Filter by permission.</param>
        /// <param name="onlineOnly">When <value>true</value>, get only users which are currently online. Defaults to <value>false</value>.</param>
        /// <param name="start">Paginator mode (takes 10 records).</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="User" /> instances.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<User>> GetUsers(string filter = null, string group = null, string role = null,
            string project = null, string permission = null, bool onlineOnly = false, int start = 0)
        {
            var queryString = new List<string>(7);
            if (!string.IsNullOrEmpty(filter))
            {
                queryString.Add($"q={WebUtility.UrlEncode(filter)}");
            }
            if (!string.IsNullOrEmpty(group))
            {
                queryString.Add($"group={WebUtility.UrlEncode(group)}");
            }
            if (!string.IsNullOrEmpty(role))
            {
                queryString.Add($"role={WebUtility.UrlEncode(role)}");
            }
            if (!string.IsNullOrEmpty(project))
            {
                queryString.Add($"project={WebUtility.UrlEncode(project)}");
            }
            if (!string.IsNullOrEmpty(permission))
            {
                queryString.Add($"permission={WebUtility.UrlEncode(permission)}");
            }
            if (onlineOnly)
            {
                queryString.Add("onlineOnly=true");
            }
            if (start > 0)
            {
                queryString.Add($"start={start}");
            }

            var query = string.Join("&", queryString);

            return await GetUsersFromPath($"admin/user?{query}");
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/PUT-User.html">Create a new user</a>.</remarks>
        /// <param name="username">Login name of the user to be created.</param>
        /// <param name="fullName">Full name of a new user.</param>
        /// <param name="email">E-mail address of the user.</param>
        /// <param name="jabber">Jabber address for the new user.</param>
        /// <param name="password">Password for the new user.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task CreateUser(string username, string fullName, string email, string jabber, string password)
        {
            var queryString = new Dictionary<string, string>(4);
            if (!string.IsNullOrEmpty(fullName))
            {
                queryString.Add("fullName", WebUtility.UrlEncode(fullName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                queryString.Add("email", WebUtility.UrlEncode(email));
            }
            if (!string.IsNullOrEmpty(jabber))
            {
                queryString.Add("jabber", WebUtility.UrlEncode(jabber));
            }
            if (!string.IsNullOrEmpty(password))
            {
                queryString.Add("password", WebUtility.UrlEncode(password));
            }


            var url = UsersUrls.GetUser(username);
            var request = new YouTrackRestRequest(url, Method.PUT);
            request.AddParameter("application/json; charset=utf-8", request.JsonSerializer.Serialize(new FormUrlEncodedContent(queryString)), ParameterType.RequestBody);
            var response = await RestClient.ExecuteTaskAsync(request);

        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/POST-User.html">Update a user</a>.</remarks>
        /// <param name="username">Login name of the user to be updated.</param>
        /// <param name="fullName">Full name of a user.</param>
        /// <param name="email">E-mail address of the user.</param>
        /// <param name="jabber">Jabber address for the user.</param>
        /// <param name="password">Password for the user.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task UpdateUser(string username, string fullName = null, string email = null, string jabber = null, string password = null)
        {
            var queryString = new Dictionary<string, string>(4);
            if (!string.IsNullOrEmpty(fullName))
            {
                queryString.Add("fullName", WebUtility.UrlEncode(fullName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                queryString.Add("email", WebUtility.UrlEncode(email));
            }
            if (!string.IsNullOrEmpty(jabber))
            {
                queryString.Add("jabber", WebUtility.UrlEncode(jabber));
            }
            if (!string.IsNullOrEmpty(password))
            {
                queryString.Add("password", WebUtility.UrlEncode(password));
            }


            var url = UsersUrls.GetUser(username);
            var request = new YouTrackRestRequest(url, Method.POST);
            request.AddParameter("application/json; charset=utf-8", request.JsonSerializer.Serialize(new FormUrlEncodedContent(queryString)), ParameterType.RequestBody);
            var response = await RestClient.ExecuteTaskAsync(request);
        }

        /// <summary>
        /// Delete specific user account.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/DELETE-User.html">Delete a user</a>.</remarks>
        /// <param name="username">Login name of the user to be deleted.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task DeleteUser(string username)
        {
            var url = UsersUrls.GetUser(username);
            var request = new YouTrackRestRequest(url, Method.DELETE);
            var response = await RestClient.ExecuteTaskAsync<User>(request);

        }

        /// <summary>
        /// Merge users.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Merge-Users.html">Delete a user</a>.</remarks>
        /// <param name="usernameToMerge">Login name of the user to be merged.</param>
        /// <param name="targetUser">Login name of the user to merge <paramref name="usernameToMerge"/> into.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task MergeUsers(string usernameToMerge, string targetUser)
        {
            var url = $"/admin/user/{targetUser}/merge/{usernameToMerge}";
            var request = new YouTrackRestRequest(url, Method.POST);
            request.AddParameter("application/json; charset=utf-8", request.JsonSerializer.Serialize(new StringContent(string.Empty)), ParameterType.RequestBody);
            var response = await RestClient.ExecuteTaskAsync(request);
        }

        /// <summary>
        /// Get all groups the specified user participates in.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-User-Groups.html">Get all groups the specified user participates in</a>.</remarks>
        /// <param name="username">Login name of the user to retrieve information for.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Group" /> instances.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<Group>> GetGroupsForUser(string username)
        {
            var url = UsersUrls.GetGroupsForUser(username);
            var request = new YouTrackRestRequest(url, Method.GET);
            var response = await RestClient.ExecuteTaskAsync<List<Group>>(request);
            return response.Data;
        }

        /// <summary>
        /// Add user to group.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/POST-User-Group.html">Add user account to a group</a>.</remarks>
        /// <param name="username">Login name of the user to be updated.</param>
        /// <param name="group">Name of the group to add the user to.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task AddUserToGroup(string username, string group)
        {
            var url = $"rest/admin/user/{username}/group/{WebUtility.UrlEncode(group)}";
            var request = new YouTrackRestRequest(url, Method.POST);
            request.AddParameter("application/json; charset=utf-8", request.JsonSerializer.Serialize(new StringContent(string.Empty)), ParameterType.RequestBody);
            var response = await RestClient.ExecuteTaskAsync(request);
        }

        /// <summary>
        /// Remove user from group.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/DELETE-User-Group.html">Remove user account from a group</a>.</remarks>
        /// <param name="username">Login name of the user to be updated.</param>
        /// <param name="group">Name of the group to remove the user from.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task RemoveUserFromGroup(string username, string group)
        {

            var url = UsersUrls.RemoveUserFromGroup(username, group);
            var request = new YouTrackRestRequest(url, Method.DELETE);
            var response = await RestClient.ExecuteTaskAsync<User>(request);

        }

        private async Task<ICollection<User>> GetUsersFromPath(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            var url = path;
            var request = new YouTrackRestRequest(url, Method.GET);
            
            var response = await RestClient.ExecuteTaskAsync<List<User>>(request);
            return response.Data;
        }
    }
}