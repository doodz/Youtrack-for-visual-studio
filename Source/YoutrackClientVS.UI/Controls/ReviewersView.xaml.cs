using System.Windows;
using System.Windows.Controls;

namespace YouTrackClientVS.UI.Controls
{
    /// <summary>
    /// Interaction logic for ReviewersView.xaml
    /// </summary>
    public partial class ReviewersView : UserControl
    {
        public ReviewersView()
        {
            InitializeComponent();
        }

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEditable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(ReviewersView), new PropertyMetadata(true));


    }


}
