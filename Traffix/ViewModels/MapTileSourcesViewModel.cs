using Microsoft.Phone.Controls.Primitives;
using System;
using System.Collections.Generic;
using Traffix.Mapping;


namespace Traffix.ViewModels
{

    public class MapTileSourcesViewModel : ViewModel, ILoopingSelectorDataSource
    {

        private readonly List<string> _mapTileSources = new List<string>();
        private int _selectedMapTileSourceIndex;


        public event EventHandler<System.Windows.Controls.SelectionChangedEventArgs> SelectionChanged;


        public MapTileSourcesViewModel()
        {
            GetMapTileSources();
        }


        #region public

        public object SelectedItem
        {

            get
            {
                return this._mapTileSources[this._selectedMapTileSourceIndex];
            }

            set
            {

                this._selectedMapTileSourceIndex = this._mapTileSources.IndexOf(value.ToString());
                base.OnPropertyChanged(x => this.SelectedItem);

                if (SelectionChanged != null)
                {
                    SelectionChanged(this, new System.Windows.Controls.SelectionChangedEventArgs(new List<object>(), new List<object>()));
                }

            }

        }

        public int SelectedItemIndex
        {

            get
            {
                return this._selectedMapTileSourceIndex;
            }

        }


        public object GetNext(object relativeTo)
        {

            this._selectedMapTileSourceIndex = this._mapTileSources.IndexOf(relativeTo.ToString()) + 1;

            if (this._selectedMapTileSourceIndex > this._mapTileSources.Count - 1)
            {
                this._selectedMapTileSourceIndex = 0;
            }

            return this._mapTileSources[this._selectedMapTileSourceIndex];

        }

        public object GetPrevious(object relativeTo)
        {

            this._selectedMapTileSourceIndex = this._mapTileSources.IndexOf(relativeTo.ToString()) - 1;

            if (this._selectedMapTileSourceIndex < 0)
            {
                this._selectedMapTileSourceIndex = this._mapTileSources.Count - 1;
            }

            return this._mapTileSources[this._selectedMapTileSourceIndex];

        }

        #endregion

        #region private

        private void GetMapTileSources()
        {

            for (int i = (int)MapTileSource.BingHybrid; i < (int)MapTileSource.YahooStreet; i++)
            {
                
                this._mapTileSources.Add(((MapTileSource)i).ToString());

                if(((MapTileSource)i) == MapTileSource.BingStreet)
                {
                    this._selectedMapTileSourceIndex = i - 1;
                }

            }

        }

        #endregion

    }

}
