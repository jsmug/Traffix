using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Traffix.Converters
{

    public class BooleanToVisibilityConverter : IValueConverter
    {

        public BooleanToVisibilityConverter()
        {
        }


        #region public

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            Visibility vis = Visibility.Visible;

            if (value.GetType() == typeof(bool))
            {

                vis = (bool)value == true ? Visibility.Visible : Visibility.Collapsed; 

            }

            return vis;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            bool vis = true;

            if (value.GetType() == typeof(Visibility))
            {

                vis = (Visibility)value == Visibility.Visible ? true : false;

            }

            return vis;

        }

        #endregion

    }

}
