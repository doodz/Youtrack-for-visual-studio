//------------------------------------------------------------------------------
// <copyright file="DiffWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using YouTrackClientVS.Infrastructure.ViewModels;
using DiffWindowControl = YouTrackClientVS.UI.Views.DiffWindowControl;

namespace YouTrackClientVS.VisualStudio.UI.Window
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("0027eeb1-cbc1-4e21-a806-e3d120f7a770")]
    public class DiffWindow : ToolWindowPane
    {
        private DiffWindowControl _view;
        private DiffWindowControlViewModel _viewModel;

        public DiffWindowControlViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                _view.DataContext = _viewModel;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiffWindow"/> class.
        /// </summary>
        public DiffWindow() : base(null)
        {
            Caption = "Diff";
            _view = new DiffWindowControl();
            Content = _view;
        }

        protected override void OnClose()
        {
            base.OnClose();

            (ViewModel?.VsFrame as IVsWindowFrame)?.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_NoSave);
        }
    }
}