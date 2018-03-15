using System;
using System.Globalization;

namespace YouTrackClientVS.UI.Converters
{
    public class StringToShortStringConverter : BaseMarkupExtensionConverter
    {
        private readonly int _maxLength;

        public StringToShortStringConverter(int maxLength)
        {
            _maxLength = maxLength;
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var str = (string) value;
            if (str.Length <= _maxLength)
                return str;

            return str.Substring(0, _maxLength);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
