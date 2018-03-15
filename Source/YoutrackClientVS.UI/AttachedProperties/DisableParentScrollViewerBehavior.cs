using System.Windows.Controls;
using System.Windows.Interactivity;
using YouTrackClientVS.UI.Helpers;

namespace YouTrackClientVS.UI.AttachedProperties
{
    public class DisableParentScrollViewerBehavior : Behavior<UserControl>
    {
        private UserControl _element;

        public bool DisableHorizontal { get; set; }
        public bool DisableVertical { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            _element = AssociatedObject;
            _element.Loaded += _element_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            _element.Loaded -= _element_Loaded;
        }

        private void _element_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var sv = VisualTreeHelpers.FindParent<ScrollViewer>(_element);
            if (sv != null)
            {
                if (DisableHorizontal)
                    sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                if (DisableVertical)
                    sv.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
        }
    }
}
