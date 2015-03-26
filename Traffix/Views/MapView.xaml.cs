using Microsoft.Phone.Controls.Maps;
using System;
using System.Windows;
using System.Windows.Controls;
using Traffix.Mapping.Entities;

namespace Traffix.Views
{
    
    public partial class MapView : UserControl
    {
        
        public MapView()
        {
            
            InitializeComponent();
            this.Loaded += (s,e) => DependencyResolver.Register(typeof(Map), this.Map);

        }


        #region private

        private void MapItemsControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            Entity ent = ((FrameworkElement)e.OriginalSource).DataContext as Entity;

            if (ent != null)
            {
                ShowDetails.Begin();
            }

        }

        private void CloseDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            HideDetails.Begin();
        }
       
        #endregion 

    }

}
