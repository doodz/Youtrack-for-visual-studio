using System.Net;

namespace YouTrack.REST.API.Helpers
{
    public static class UsersUrls
    {
        public static string GetUser(string username)
        {
            return $"admin/user/{username}";
        }

        public static string GetGroupsForUser(string username)
        {
            return $"rest/admin/user/{username}/group";
        }

        public static string RemoveUserFromGroup(string username, string group)
        {
            return $"rest/admin/user/{username}/group/{WebUtility.UrlEncode(group)}";
        }
    }
}