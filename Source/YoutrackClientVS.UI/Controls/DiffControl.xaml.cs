using System.Windows.Controls;
using System.Windows.Input;

namespace YouTrackClientVS.UI.Controls
{
    /// <summary>
    /// Interaction logic for DiffControl.xaml
    /// </summary>
    public partial class DiffControl : UserControl
    {
        public DiffControl()
        {
            InitializeComponent();
        }


        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
