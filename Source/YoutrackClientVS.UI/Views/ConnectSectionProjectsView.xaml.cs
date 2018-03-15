using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;

namespace YouTrackClientVS.UI.Views
{
    public partial class ConnectSectionProjectsView : UserControl
    {
        public ConnectSectionProjectsView()
        {
            InitializeComponent();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ChangeActiveProject();
        }

        private void ListBox_MouseClickFromContext(object sender, RoutedEventArgs e)
        {
            ChangeActiveProject();
        }

        private void ChangeActiveProject()
        {
            GetViewModel()?.ChangeActiveProject();
        }

        private void FileExplorer_OnClick(object sender, RoutedEventArgs e)
        {
            //var localPath = GetViewModel()?.SelectedProject?.LocalPath;
            //if (localPath != null)
            //    Process.Start(localPath);
        }

        private IConnectSectionViewModel GetViewModel()
        {
            return DataContext as IConnectSectionViewModel;
        }

        private void CommandPrompt_OnClick(object sender, RoutedEventArgs e)
        {
            //var localPath = GetViewModel()?.SelectedProject?.LocalPath;
            //if (localPath != null)
            //{
            //    var proc1 = new ProcessStartInfo
            //    {
            //        UseShellExecute = true,
            //        WorkingDirectory = localPath,
            //        FileName = @"cmd.exe",
            //        WindowStyle = ProcessWindowStyle.Normal
            //    };

            //    Process.Start(proc1);
            //}
        }
    }
}