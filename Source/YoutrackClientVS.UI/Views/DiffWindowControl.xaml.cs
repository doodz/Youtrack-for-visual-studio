//------------------------------------------------------------------------------
// <copyright file="DiffWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Windows.Controls;
using YouTrackClientVS.Contracts.Interfaces.Views;

namespace YouTrackClientVS.UI.Views
{
    [Export(typeof(IDiffWindowControl))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DiffWindowControl : UserControl, IDiffWindowControl
    {
        [ImportingConstructor]
        public DiffWindowControl()
        {
            InitializeComponent();
        }
    }
}