using Microsoft.Phone.Controls.Maps;
using System;
using System.Windows;
using Traffix.Mapping;

namespace Traffix.Behaviors
{

    public class MapBehaviors
    {

        public static readonly DependencyProperty MapTileSourceProperty = DependencyProperty.RegisterAttached("MapTileSource", typeof(MapTileSource), typeof(MapBehaviors), new PropertyMetadata(MapTileSource.BingStreet, OnMapTileSourceChanged));


        #region public

        public static bool GetMapTileSource(Map target)
        {

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            return (bool)target.GetValue(MapTileSourceProperty);

        }

        public static void SetMapTileSource(Map target, bool value)
        {

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            target.SetValue(MapTileSourceProperty, value);

        }

        #endregion

        #region private

        private static void OnMapTileSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            Map map = d as Map;

            if (map != null)
            {
                map.SetMapTileSource((MapTileSource)e.NewValue);
            }

        }

        #endregion

    }

}
