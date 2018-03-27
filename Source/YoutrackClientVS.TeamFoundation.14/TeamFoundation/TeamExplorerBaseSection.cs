// <copyright file="TeamExplorerBaseSection.cs" company="Microsoft Corporation">Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.) This is sample code only, do not use in production environments.</copyright>

using Microsoft.TeamFoundation.Controls;
using System;
using YouTrackClientVS.Contracts.Interfaces;

namespace YouTrackClientVS.TeamFoundation.TeamFoundation
{
    /// <summary>
    /// Team Explorer base section class.
    /// </summary>
    public class TeamExplorerBaseSection : TeamExplorerBase, ITeamExplorerSection
    {
        private string _title;
        private bool _isExpanded = true;
        private bool _isVisible = true;
        private bool _isBusy;
        private object _sectionContent;

        public TeamExplorerBaseSection(IView view)
        {
            SectionContent = view;
        }

        public string Title
        {
            get => _title;

            set => SetProperty(ref _title, value);
        }

        public object SectionContent
        {
            get => _sectionContent;

            set => SetProperty(ref _sectionContent, value);
        }

        public bool IsVisible
        {
            get => _isVisible;

            set => SetProperty(ref _isVisible, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;

            set => SetProperty(ref _isExpanded, value);
        }

        public bool IsBusy
        {
            get => _isBusy;

            set => SetProperty(ref _isBusy, value);
        }

        public virtual void Initialize(object sender, SectionInitializeEventArgs e)
        {
            ServiceProvider = e.ServiceProvider;
        }

        public virtual void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
        }

        public virtual void Loaded(object sender, SectionLoadedEventArgs e)
        {
        }

        public virtual void Refresh()
        {
        }

        public virtual void Cancel()
        {
        }

        public virtual object GetExtensibilityService(Type serviceType)
        {
            return null;
        }
    }
}