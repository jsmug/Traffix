using Microsoft.Phone.Controls.Maps;
using System;

namespace Traffix.Mapping.TileSources
{

    public class GoogleMapTileSource : TileSource
    {

        #region constants

        private const string HybridTileUri = "http://mt{0}.google.com/vt/lyrs=y&z={1}&x={2}&y={3}";
        private const string PhysicalTileUri = "http://mt{0}.google.com/vt/lyrs=t&z={1}&x={2}&y={3}";
        private const string SatelliteTileUri = "http://mt{0}.google.com/vt/lyrs=s&z={1}&x={2}&y={3}";
        private const string StreetTileUri = "http://mt{0}.google.com/vt/lyrs=m&z={1}&x={2}&y={3}";
        private const string WaterOverlayTileUri = "http://mt{0}.google.com/vt/lyrs=r&z={1}&x={2}&y={3}";
        
        #endregion

        #region enums

        public enum GoogleMapMode
        {
            Hybrid = 1,
            Physical = 2,
            Satellite = 3,
            Street = 0,
            WaterOverlay = 4
        }

        #endregion


        public GoogleMapTileSource(GoogleMapMode googleMapMode) : base(default(string))
        {

            this.Server = 0;
            base.UriFormat = MapModeToUri(googleMapMode);

        }


        #region public

        public int Server { get; set; }


        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            return new Uri(string.Format(System.Globalization.CultureInfo.InvariantCulture, UriFormat, this.Server, zoomLevel, x, y));
        }

        #endregion

        #region private

        private string MapModeToUri(GoogleMapMode yahooMapMode)
        {

            string uri = default(string);

            switch (yahooMapMode)
            {

                case GoogleMapMode.Hybrid:
                    uri = HybridTileUri;
                    break;

                case GoogleMapMode.Physical:
                    uri = PhysicalTileUri;
                    break;

                case GoogleMapMode.Satellite:
                    uri = SatelliteTileUri;
                    break;

                case GoogleMapMode.Street:
                    uri = StreetTileUri;
                    break;

                case GoogleMapMode.WaterOverlay:
                    uri = WaterOverlayTileUri;
                    break;

                default:
                    uri = StreetTileUri;
                    break;

            }

            return uri;

        }

        #endregion

    }

}
