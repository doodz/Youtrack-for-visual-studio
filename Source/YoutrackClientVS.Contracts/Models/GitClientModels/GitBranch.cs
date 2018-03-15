namespace YouTrackClientVS.Contracts.Models.GitClientModels
{
    public class GitBranch
    {
        public GitCommit Target { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }
}