using System.Collections.Generic;
using ParseDiff;
using YouTrackClientVS.Contracts.Models.GitClientModels;
using YouTrackClientVS.Contracts.Models.Tree;

namespace YouTrackClientVS.Contracts.Interfaces
{
    public interface ITreeStructureGenerator
    {
        IEnumerable<ICommentTree> CreateCommentTree(IEnumerable<GitComment> gitComments, char separator = '/');
        IEnumerable<ITreeFile> CreateFileTree(IEnumerable<FileDiff> fileDiffs, string rootFileName = "test", char separator = '/');
    }
}