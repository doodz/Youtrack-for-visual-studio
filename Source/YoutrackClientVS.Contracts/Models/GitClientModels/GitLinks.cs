using System.Collections.Generic;

namespace YouTrackClientVS.Contracts.Models.GitClientModels
{
    public class GitLinks
    {
        public GitLink Self { get; set; }
        public GitLink Repositories { get; set; }
        public GitLink Link { get; set; }
        public GitLink Followers { get; set; }
        public GitLink Avatar { get; set; }
        public GitLink Following { get; set; }
        public List<GitLink> Clone { get; set; }
    }
}
