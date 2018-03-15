using System;
using System.Globalization;

namespace YouTrackClientVS.UI.Converters
{
    public class SubstractConverter : BaseMarkupExtensionConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var a = System.Convert.ToDouble(value);
            var b = System.Convert.ToDouble(parameter);
            return a - b;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
