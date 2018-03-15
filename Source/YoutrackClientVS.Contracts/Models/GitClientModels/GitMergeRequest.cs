namespace YouTrackClientVS.Contracts.Models.GitClientModels
{
    public class GitMergeRequest
    {
        public long Id { get; set; }
        public bool? CloseSourceBranch { get; set; }
        public string MergeStrategy { get; set; } //merge_commit, squash, todo enum
        public string Version { get; set; }
    }
}
