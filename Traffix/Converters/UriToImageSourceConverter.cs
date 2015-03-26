using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Traffix.Converters
{

    public class UriToImageSourceConverter : IValueConverter
    {

        public UriToImageSourceConverter()
        {
        }


        #region public

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            BitmapImage img = null;

            try 
            { 
                img = new BitmapImage((Uri)value); 
            }
            catch
            { 
                img = new BitmapImage(); 
            }

            return img;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
