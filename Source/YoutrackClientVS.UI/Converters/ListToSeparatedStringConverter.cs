using System;
using System.Collections.Generic;
using System.Globalization;

namespace YouTrackClientVS.UI.Converters
{
    public class ListToSeparatedStringConverter : BaseMarkupExtensionConverter
    {
        private readonly string _delimiter;

        public ListToSeparatedStringConverter() : this(","){ }

        public ListToSeparatedStringConverter(string delimiter)
        {
            _delimiter = delimiter;
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return string.Join(_delimiter, (IEnumerable<object>)value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return ((string)value).Split(new[] { _delimiter }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
