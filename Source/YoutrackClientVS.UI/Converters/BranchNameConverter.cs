using System;
using System.Globalization;
using System.Linq;

namespace YouTrackClientVS.UI.Converters
{
    public class BranchNameConverter : BaseMarkupExtensionConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var branchName = (string)value;
            if (string.IsNullOrEmpty(branchName))
                return null;

            return branchName.Split('/').LastOrDefault();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
