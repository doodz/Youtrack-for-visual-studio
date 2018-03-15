using GitClientVS.UI.Views;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace GitClientVS.VisualStudio.UI.Window
{
    [Guid("E39DFA69-39C9-4C8E-A3D6-C1658AA749DA")]
    public class YouTrackIssuesWindows : ToolWindowPane
    {


        public YouTrackIssuesWindows() : base(null)
        {
            this.Caption = "YouTrack - Issues";
            Content = new YouTrackIssuesWindowsContainer();
        }
    }
}
