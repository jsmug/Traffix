using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Traffix.Mapping
{

    public enum DistanceUnits
    {
        Kilometers = 1,
        Meters = 0,
        Miles = 2
    }

    public enum MapTileSource
    {
        None = 0,
        BingHybrid = 1,
        BingSatellite = 2,
        BingStreet = 3,
        GoogleHybrid = 4,
        GooglePhysical = 5,
        GoogleSatellite = 6,
        GoogleStreet = 7,
        GoogleWaterOverlay = 8,
        OpenStreetMap = 9,
        YahooHybrid = 10,
        YahooSatellite = 11,
        YahooStreet = 12
    }

}
