using System;
using System.Globalization;
using System.Windows;

namespace YouTrackClientVS.UI.Converters
{
    public class BoolToVisibilityConverter : BaseMarkupExtensionConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return ((Visibility) value) == Visibility.Visible;
        }
    }
}
