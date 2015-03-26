using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Core;
using Traffix.Mapping.TileSources;

namespace Traffix.Mapping
{

    public static class MapExtensions
    {

        #region public

        public static void SetMapTileSource(this Map map, MapTileSource tileSource)
        {

            MapTileLayer layer = null;
            MapTileLayer currentMapTileLayer = null;
            bool isBing = false;

            if (map != null)
            {

                map.Mode = new MercatorMode();
                layer = new MapTileLayer();

                switch (tileSource)
                {

                    case MapTileSource.BingHybrid:
                        isBing = true;
                        map.Mode = new AerialMode() { ShouldDisplayLabels = true };
                        break;

                    case MapTileSource.BingSatellite:
                        isBing = true;
                        map.Mode = new AerialMode();
                        break;

                    case MapTileSource.BingStreet:
                        isBing = true;
                        map.Mode = new RoadMode();
                        break;

                    case MapTileSource.GoogleHybrid:
                        layer.TileSources.Add(new GoogleMapTileSource(GoogleMapTileSource.GoogleMapMode.Hybrid));
                        break;

                    case MapTileSource.GooglePhysical:
                        layer.TileSources.Add(new GoogleMapTileSource(GoogleMapTileSource.GoogleMapMode.Physical));
                        break;

                    case MapTileSource.GoogleSatellite:
                        layer.TileSources.Add(new GoogleMapTileSource(GoogleMapTileSource.GoogleMapMode.Satellite));
                        break;

                    case MapTileSource.GoogleStreet:
                        layer.TileSources.Add(new GoogleMapTileSource(GoogleMapTileSource.GoogleMapMode.Street));
                        break;

                    case MapTileSource.GoogleWaterOverlay:
                        layer.TileSources.Add(new GoogleMapTileSource(GoogleMapTileSource.GoogleMapMode.WaterOverlay));
                        break;

                    case MapTileSource.OpenStreetMap:
                        layer.TileSources.Add(new OpenStreetMapTileSource());
                        break;

                    case MapTileSource.YahooHybrid:
                        layer.TileSources.Add(new YahooMapTileSource(YahooMapTileSource.YahoMapMode.Hybrid));
                        break;

                    case MapTileSource.YahooSatellite:
                        layer.TileSources.Add(new YahooMapTileSource(YahooMapTileSource.YahoMapMode.Satellite));
                        break;

                    case MapTileSource.YahooStreet:
                        layer.TileSources.Add(new YahooMapTileSource(YahooMapTileSource.YahoMapMode.Street));
                        break;

                    default:
                        break;

                }

                for(int i = map.Children.Count - 1; i >= 0; i--)
                {
                
                    currentMapTileLayer = map.Children[i] as MapTileLayer;
 
                    if(currentMapTileLayer != null)
                    {
                        map.Children.RemoveAt(i);
                    }

                }

                if (!isBing)
                {
                    map.Children.Insert(0, layer);
                }

            }

        }

        #endregion

    }

}
