using Microsoft.Phone.Controls.Maps;
using System;

namespace Traffix.Mapping.TileSources
{

    public class OpenStreetMapTileSource : TileSource
    {

        #region constants

        private const string TileUri = "http://tile.openstreetmap.org/{2}/{0}/{1}.png";

        #endregion


        public OpenStreetMapTileSource() : base(TileUri)
        {
        }


        #region public

        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            return new Uri(string.Format(System.Globalization.CultureInfo.InvariantCulture, base.UriFormat, x, y, zoomLevel));
        }

        #endregion

    }

}
