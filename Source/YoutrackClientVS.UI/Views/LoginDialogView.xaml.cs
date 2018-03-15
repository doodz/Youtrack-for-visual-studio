using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using YouTrackClientVS.Contracts.Interfaces.ViewModels;
using YouTrackClientVS.Contracts.Interfaces.Views;
using YouTrackClientVS.Infrastructure.Extensions;

namespace YouTrackClientVS.UI.Views
{
    /// <summary>
    /// Interaction logic for LoginDialogView.xaml
    /// </summary>
    [Export(typeof(ILoginDialogView))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class LoginDialogView : MetroWindow, ILoginDialogView
    {
        private ControlTemplate _actualPbTemplate;

        [ImportingConstructor]
        public LoginDialogView(ILoginDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            Owner = Application.Current.MainWindow;
            vm.Closed += delegate { Close(); };
            Loaded += LoginDialogView_Loaded;
            vm.Initialize();
        }

        private void LoginDialogView_Loaded(object sender, RoutedEventArgs e)
        {
            _actualPbTemplate = Validation.GetErrorTemplate(PasswordBox);
            Validation.SetErrorTemplate(PasswordBox, new ControlTemplate());
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Validation.SetErrorTemplate(PasswordBox, _actualPbTemplate);
        }
    }
}