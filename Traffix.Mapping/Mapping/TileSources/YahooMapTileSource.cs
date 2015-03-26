using Microsoft.Phone.Controls.Maps;
using System;

namespace Traffix.Mapping.TileSources
{

    public class YahooMapTileSource : TileSource
    {

        #region constants

        private const string HybridTileUri = "http://us.maps3.yimg.com/aerial.maps.yimg.com/png?v=2.2&t=h&x={0}&y={1}&z={2}";
        private const string SatelliteTileUri = "http://us.maps3.yimg.com/aerial.maps.yimg.com/tile?v=1.7&t=a&x={0}&y={1}&z={2}";
        private const string StreetTileUri = "http://us.maps2.yimg.com/us.png.maps.yimg.com/png?v=3.52&t=m&x={0}&y={1}&z={2}";
        
        #endregion

        #region enums

        public enum YahoMapMode
        {
            Hybrid = 1,
            Satellite = 2,
            Street = 0
        }

        #endregion


        public YahooMapTileSource(YahoMapMode yahooMapMode) : base(default(string))
        {

            base.UriFormat = MapModeToUri(yahooMapMode);

        }


        #region public

        public override Uri GetUri(int x, int y, int zoomLevel)
        {

            double posY = default(double);
            double zoom = 18 - zoomLevel;
            double num = Math.Pow(2.0, zoomLevel) / 2.0;

            if (y < num)
            {
                posY = (num - Convert.ToDouble(y)) - 1.0;
            }
            else
            {
                posY = ((Convert.ToDouble(y) + 1) - num) * -1.0;
            }

            return new Uri(string.Format(System.Globalization.CultureInfo.InvariantCulture, UriFormat, x, posY, zoom));

        }

        #endregion

        #region private

        private string MapModeToUri(YahoMapMode yahooMapMode)
        {

            string uri = default(string);

            switch(yahooMapMode)
            {

                case YahoMapMode.Hybrid:
                    uri = HybridTileUri;
                    break;

                case YahoMapMode.Satellite:
                    uri = SatelliteTileUri;
                    break;

                case YahoMapMode.Street:
                    uri = StreetTileUri;
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
