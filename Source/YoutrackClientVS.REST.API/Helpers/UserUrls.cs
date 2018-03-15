namespace YouTrack.REST.API.Helpers
{
    public static class UserUrls
    {

        public static string CurrentUserInfo()
        {
            return "user/current";
        }

        public static string Login(string login, string password)
        {
            return $"user/login?login={login}&password={password}";
        }
        public static string Login()
        {
            return $"user/login";
        }
    }
}
