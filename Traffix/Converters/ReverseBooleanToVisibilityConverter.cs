using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Traffix.Converters
{

    public class ReverseBooleanToVisibilityConverter : IValueConverter
    {

        public ReverseBooleanToVisibilityConverter()
        {
        }


        #region public

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            Visibility vis = Visibility.Collapsed;

            if (value.GetType() == typeof(bool))
            {

                vis = (bool)value == true ? Visibility.Collapsed : Visibility.Visible;

            }

            return vis;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            bool vis = false;

            if (value.GetType() == typeof(Visibility))
            {

                vis = (Visibility)value == Visibility.Visible ? false : true;

            }

            return vis;

        }

        #endregion

    }

}
