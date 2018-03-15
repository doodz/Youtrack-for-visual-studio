using System.Windows;
using System.Windows.Controls;

namespace YouTrackClientVS.UI.Controls
{
    /// <summary>
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class ProgressBar : UserControl
    {
        public double? OverrideWidth
        {
            get => (double?)GetValue(OverrideWidthProperty);
            set => SetValue(OverrideWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for OverrideWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverrideWidthProperty =
            DependencyProperty.Register(nameof(OverrideWidth), typeof(double?), typeof(ProgressBar),
                new PropertyMetadata(null));

        public double? OverrideHeight
        {
            get => (double?)GetValue(OverrideHeightProperty);
            set => SetValue(OverrideHeightProperty, value);
        }

        // Using a DependencyProperty as the backing store for OverrideHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverrideHeightProperty =
            DependencyProperty.Register(nameof(OverrideHeight), typeof(double?), typeof(ProgressBar),
                new PropertyMetadata(null));


        public UIElement ProgressContent
        {
            get => (UIElement)GetValue(ProgressContentProperty);
            set => SetValue(ProgressContentProperty, value);
        }

        // Using a DependencyProperty as the backing store for ProgressContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressContentProperty =
            DependencyProperty.Register(nameof(ProgressContent), typeof(UIElement), typeof(ProgressBar),
                new PropertyMetadata(null, OnProgressContentChanged));

        private static void OnProgressContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as ProgressBar;
            obj?.RefreshVisibility();
        }

        public ProgressBar()
        {
            InitializeComponent();
            IsVisibleChanged += ProgressBar_IsVisibleChanged;
            Loaded += ProgressBar_Loaded;
        }

        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (OverrideHeight != null)
                ProgressRing.Height = OverrideHeight.Value;
            if (OverrideWidth != null)
                ProgressRing.Width = OverrideWidth.Value;
        }

        private void ProgressBar_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshVisibility();
        }

        private void RefreshVisibility()
        {
            if (ProgressContent != null)
                ProgressContent.Visibility = Visibility == Visibility.Visible
                    ? Visibility.Collapsed
                    : Visibility.Visible;
        }
    }
}