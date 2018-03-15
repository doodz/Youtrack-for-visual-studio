namespace YouTrack.REST.API.Helpers
{
    public static class ProjectsUrls
    {


        public static string AccessibleProjects(bool verbose = false)
        {
            return $"project/all?verbose={verbose}";
        }
    }
}