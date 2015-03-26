using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Traffix.Converters
{

    public class LocationDataToCityStateZipConverter : IValueConverter
    {

        public LocationDataToCityStateZipConverter()
        {
        }


        #region public

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            string address = "Unknown";
            Traffix.Mapping.LocationData locationData = null;

            if (value.GetType() == typeof(Traffix.Mapping.LocationData))
            {

                locationData = (Traffix.Mapping.LocationData)value;
                address = string.Format("{0}, {1} {2}", locationData.City, locationData.State, locationData.Zip);

            }

            return address;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
