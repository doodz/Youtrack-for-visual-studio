//------------------------------------------------------------------------------
// <copyright file="DiffWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;

namespace YouTrackClientVS.UI.Views
{
    [Export(typeof(IYouTrackIssuesWindowContainer))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class YouTrackIssuesWindowContainer : UserControl, IYouTrackIssuesWindowContainer
    {

        [ImportingConstructor]
        public YouTrackIssuesWindowContainer()
        {
            InitializeComponent();
            DataContextChanged += PullRequestsWindowContainer_DataContextChanged;
        }

        private void PullRequestsWindowContainer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel != null)
                ViewModel.Closed -= CloseWindow;

            ViewModel = DataContext as IYouTrackIssuesWindowContainerViewModel;

            if (ViewModel != null)
                ViewModel.Closed += CloseWindow;
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Window.Close();
        }

        public IYouTrackIssuesWindowContainerViewModel ViewModel { get; set; }
        public IYouTrackIssuesWindow Window { get; set; }
    }
}