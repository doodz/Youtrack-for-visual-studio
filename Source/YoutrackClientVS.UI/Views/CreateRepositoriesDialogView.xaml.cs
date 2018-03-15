using System.ComponentModel.Composition;
using System.Windows;
using MahApps.Metro.Controls;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.UI.Views
{
    /// <summary>
    /// Interaction logic for CloneRepositoriesDialogView.xaml
    /// </summary>
    [Export(typeof(ICreateRepositoriesDialogView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CreateRepositoriesDialogView : MetroWindow, ICreateRepositoriesDialogView
    {

        [ImportingConstructor]
        public CreateRepositoriesDialogView(ICreateRepositoriesDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            Owner = Application.Current.MainWindow;
            vm.Closed += delegate { Close(); };
            vm.Initialize();
        }
    }
}
