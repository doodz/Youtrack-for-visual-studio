using YouTrackClientVS.Contracts.Models.GitClientModels;

namespace YouTrackClientVS.Contracts.Models.Tree
{
    public class ObjectTree
    {
        public string Path { get; set; }
        public GitComment GitComment { get; set; }

        public ObjectTree(string path, GitComment gitComment)
        {
            Path = path;
            GitComment = gitComment;
        }
    }
}