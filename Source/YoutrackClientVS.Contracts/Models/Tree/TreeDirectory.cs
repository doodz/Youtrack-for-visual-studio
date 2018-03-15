using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.PlatformUI;
using ParseDiff;

namespace YouTrackClientVS.Contracts.Models.Tree
{
    public class TreeDirectory : ObservableObject, ITreeFile
    {
        private bool _isSelected;
        private bool _isExpanded;
        public string Name { get; set; }
        public ICollection<ITreeFile> Files { get; private set; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public long Comments { get; set; }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        public bool IsAdded { get; set; }
        public bool IsRemoved { get; set; }
        public FileDiff FileDiff { get; set; }

        public Type GetTreeType => GetType();

        public bool IsSelectable { get; set; }
        public long Added { get; set; }
        public long Removed { get; set; }

        public TreeDirectory(string name)
        {
            Name = name;
            Files = new List<ITreeFile>();
            FileDiff = null;
            IsSelectable = false;
        }
    }
}