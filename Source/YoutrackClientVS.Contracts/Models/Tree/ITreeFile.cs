using System;
using System.Collections.Generic;
using ParseDiff;

namespace YouTrackClientVS.Contracts.Models.Tree
{
    public interface ITreeFile
    {
        string Name { get; set; }
        ICollection<ITreeFile> Files { get; }
        FileDiff FileDiff { get; set; }
        bool IsAdded { get; set; }
        bool IsRemoved { get; set; }
        long Added { get; set; }
        long Removed { get; set; }
        long Comments { get; set; }
        bool IsExpanded { get; set; }

        bool IsSelectable { get; set; }
        Type GetTreeType { get; }
    }
}