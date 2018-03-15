using System.ComponentModel.Composition;
using MahApps.Metro.Controls;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.UI.Views
{
    /// <summary>
    /// Interaction logic for CloneRepositoriesDialogView.xaml
    /// </summary>
    [Export(typeof(ICloneRepositoriesDialogView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class CloneRepositoriesDialogView : MetroWindow, ICloneRepositoriesDialogView
    {
        [ImportingConstructor]
        public CloneRepositoriesDialogView(ICloneRepositoriesDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.Closed += delegate { Close(); };
            vm.Initialize();
        }
    }
}
