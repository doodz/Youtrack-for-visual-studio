using YouTrack.REST.API.Models.Standard;

namespace YouTrack.REST.API.Interfaces
{
    public interface IYouTrackClient
    {
        ITeamsClient TeamsClient { get; }
        IRepositoriesClient RepositoriesClient { get; }
        IUserClient UserClient { get; }
        IPullRequestsClient PullRequestsClient { get; }
        Connection ApiConnection { get; }


        IProjectsClient ProjectsClient { get; }
        IIssuesClient IssuesClient { get; }
        ITimeTrackingClient TimeTrackingClient { get; }
        IUserManagementClient UserManagementClient { get; }
        ITimeTrackingManagementClient TimeTrackingManagementClient { get; }
        IProjectCustomFieldsClient ProjectCustomFieldsClient { get; }
        IAgileBoardClient AgileBoardClient { get; }
    }
}