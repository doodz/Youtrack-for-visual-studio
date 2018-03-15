using System.Collections.Generic;
using System.Linq;
using YouTrackClientVS.Contracts.Models.Tree;

namespace YouTrackClientVS.Infrastructure
{
    public class CommentTreeCollection
    {
        public IEnumerable<ICommentTree> Elements { get; private set; }

        public CommentTreeCollection(IEnumerable<ICommentTree> commentTree)
        {
            Elements = commentTree.ToList();
        }
    }
}