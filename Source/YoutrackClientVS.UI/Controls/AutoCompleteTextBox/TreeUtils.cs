using System.Windows;
using System.Windows.Media;

namespace YouTrackClientVS.UI.Controls.AutoCompleteTextBox
{
    internal static class TreeUtils
    {
        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (!(child is T childType))
                {
                    foundChild = FindChild<T>(child);
                    if (foundChild != null) break;
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static T FindParent<T>(DependencyObject startingObject) where T : DependencyObject
        {
            var parent = GetParent(startingObject);

            while (parent != null)
            {
                if (parent is T foundElement) return foundElement;

                parent = GetParent(parent);
            }

            return null;
        }

        private static DependencyObject GetParent(DependencyObject element)
        {
            var parent = !(element is Visual visual) ? null : VisualTreeHelper.GetParent(visual);

            if (parent == null)
            {
                // Check for a logical parent when no visual was found.

                if (element is FrameworkElement frameworkElement)
                {
                    parent = frameworkElement.Parent ?? frameworkElement.TemplatedParent;
                }
                else
                {
                    if (element is FrameworkContentElement frameworkContentElement) parent = frameworkContentElement.Parent ?? frameworkContentElement.TemplatedParent;
                }
            }

            return parent;
        }
    }
}