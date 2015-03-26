using Microsoft.Phone.Controls.Maps.Core;
using System;
using System.Device.Location;

namespace Traffix.Mapping.Utilities
{

    public static class Haversine
    {

        #region public

        public static double GetDistance(GeoCoordinate startLocation, GeoCoordinate endLocation, DistanceUnits units)
        {

            double radius = default(double);
            double deltaLat = default(double);
            double deltaLon = default(double);
            double a = default(double);
            double c = default(double);

            switch(units)
            {

                case DistanceUnits.Kilometers:
                    radius = 6371;
                    break;

                case DistanceUnits.Meters:
                    radius = 6371000;
                    break;

                case  DistanceUnits.Miles:
                    radius = 3960;
                    break;

                default:
                    radius = 6371000;
                    break;

            }


            //Haversine formula for giving great-arc circle distances between two points on a sphere
            deltaLat = MercatorUtility.DegreesToRadians(endLocation.Latitude - startLocation.Latitude);
            deltaLon = MercatorUtility.DegreesToRadians(endLocation.Longitude - startLocation.Longitude);
            
            a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) + Math.Cos(MercatorUtility.DegreesToRadians(startLocation.Latitude)) * Math.Cos(MercatorUtility.DegreesToRadians(endLocation.Latitude)) * Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
 
            return radius * c;

        }

        #endregion

    }

}
