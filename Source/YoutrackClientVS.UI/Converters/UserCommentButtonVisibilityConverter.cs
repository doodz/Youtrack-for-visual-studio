using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using YouTrackClientVS.Contracts.Models.GitClientModels;

namespace YouTrackClientVS.UI.Converters
{
    public class UserCommentButtonVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var gitUser = values[0] as string;
            var gitComment = values[1] as GitComment;

            if (gitComment == null || gitUser == null)
                return Visibility.Collapsed;

            return gitComment.User.Username == gitUser ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
