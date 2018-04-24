using YouTrack.REST.API.Clients.Enterprise;
using YouTrack.REST.API.Clients.Standard;
using YouTrack.REST.API.Interfaces;
using YouTrack.REST.API.Models.Standard;
using YouTrack.REST.API.Wrappers;

namespace YouTrack.REST.API
{
    public class EnterpriseYouTrackClient : IYouTrackClient
    {

        public Connection ApiConnection { get; }

        public ITeamsClient TeamsClient { get; }
        public IRepositoriesClient RepositoriesClient { get; }

        public IPullRequestsClient PullRequestsClient { get; }


        public IUserClient UserClient { get; }
        public IProjectsClient ProjectsClient { get; }
        public IIssuesClient IssuesClient { get; }

        public ITimeTrackingClient TimeTrackingClient { get; }
        public IUserManagementClient UserManagementClient { get; }
        public ITimeTrackingManagementClient TimeTrackingManagementClient { get; }
        public IProjectCustomFieldsClient ProjectCustomFieldsClient { get; }
        public IAgileBoardClient AgileBoardClient { get; }

        public EnterpriseYouTrackClient(Connection apiConnection)
        {
            ApiConnection = apiConnection;
            var client = new EnterpriseYouTrackRestClient(apiConnection);
            //RepositoriesClient = new EnterpriseRepositoriesClient(client, ApiConnection);
            UserClient = new EnterpriseUserClient(client, ApiConnection);
            ProjectsClient = new ProjectsClient(client, ApiConnection);
            IssuesClient = new IssuesClient(client, ApiConnection);

            UserManagementClient = new UserManagementClient(client, ApiConnection);


            //PullRequestsClient = new EnterprisePullRequestsClient(client, ApiConnection);
            //TeamsClient = new EnterpriseTeamsClient(client, ApiConnection);
        }
    }
}