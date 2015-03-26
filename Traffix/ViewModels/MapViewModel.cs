using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Reactive;
using System;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using Traffix.Mapping;
using Traffix.Mapping.Entities;
using Traffix.Mapping.Services;

namespace Traffix.ViewModels
{
    
    public class MapViewModel : ViewModel
    {

        #region constants

        private const string ADSBingMapsKey = "AvW6QHDPQOrungIu1_O1iuxhXjEcm37ESgsofldYWQDm8ezbzP2H5sjLjskfv6DB";

        #endregion


        private readonly ObservableCollection<Entity> _entities = new ObservableCollection<Entity>();
        private IDisposable _entityObserver;
        private GeoCoordinate _lastCoordinate;
        private LocationService _locationService;
        private Map _map;
        private MapTileSourcesViewModel _mapTileSources;
        private bool _mapViewChangeFromTileSource;
        private IDisposable _mapPushpinSelectObserver;
        private IDisposable _mapViewObserver;
        private EntityProvider _provider;
        private Entity _selectedEntity;
        private ShellViewModel _settings;
        private bool _tileSourceChangedFromProperty;
        private bool _useWatcherForLocation;


        public MapViewModel() : base()
        {
            Initialize();
        }


        #region public

        public LocationData CurrentLocationData
        {

            get
            {
                return (this.LocationService == null || this._locationService.LastLocationData == null) ? new LocationData() : this.LocationService.LastLocationData;
            }

        }

        public ObservableCollection<Entity> Entities
        {

            get
            {
                return this._entities;
            }

        }

        public double MapScale
        {

            get
            {
                return this._map.ZoomLevel;
            }

            set
            {

                if (this._map != null)
                {
                    this._map.ZoomLevel = value;
                }

            }

        }

        public MapTileSource TileSource
        {

            get
            {
                return (MapTileSource)this._mapTileSources.SelectedItemIndex + 1;
            }

            set
            {

                if ((MapTileSource)this._mapTileSources.SelectedItemIndex + 1 != value)
                {

                    this._tileSourceChangedFromProperty = true;
                    this._mapTileSources.SelectedItem = value;
                    base.OnPropertyChanged(x => this.TileSource);

                }

            }

        }

        public MapTileSourcesViewModel MapTileSources
        {

            get
            {
                return this._mapTileSources;
            }

        }

        public Entity SelectedEntity
        {

            get
            {
                return this._selectedEntity;
            }

            set
            {

                this._selectedEntity = value;
                base.OnPropertyChanged(x => this.SelectedEntity);

            }

        }

        #endregion

        #region private

        private LocationService LocationService
        {

            get
            {
                return this._locationService;
            }

            set
            {
                this._locationService = value;
            }

        }


        private void AddLocationWatcher()
        {

            IGeoPositionWatcher<GeoCoordinate> watcher = DependencyResolver.Resolve<IGeoPositionWatcher<GeoCoordinate>>();
            watcher.StatusChanged += LocationStatusChanged;

            if (watcher != null)
            {
                watcher.TryStart(true, TimeSpan.FromSeconds(10));
            }

        }

        private void EntityUpdated(EntityEvent entityEvent)
        {

            if (entityEvent.EntityUpdateType == EntityProvider.EntityUpdateType.Removed)
            {
                this.Entities.Remove(entityEvent.Entity);
            }
            else
            {


                if ((from Entity ent in this.Entities where string.Compare(ent.Id, entityEvent.Entity.Id, StringComparison.Ordinal) == 0 select ent).FirstOrDefault() != null)
                {
                    this.Entities[this.Entities.IndexOf(entityEvent.Entity)] = entityEvent.Entity;
                }
                else
                {
                    this.Entities.Add(entityEvent.Entity);
                }

            }

        }

        private void GetLocationDataCompleted(object sender, GetLocationCompletedEventArgs e)
        {

            base.OnPropertyChanged(x => this.CurrentLocationData);

            if (this._provider != null && !this._provider.IsRunning)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(state => { this._provider.Start(); });
            }
   
        }

        private void GetLocationData()
        {

            IGeoPositionWatcher<GeoCoordinate> watcher = DependencyResolver.Resolve<IGeoPositionWatcher<GeoCoordinate>>();

            if(watcher != null)
            {

                if (this.LocationService == null)
                {

                    this.LocationService = DependencyResolver.Resolve<LocationService>();
                    this.LocationService.GetLocationCompleted += GetLocationDataCompleted;

                }

                if (this._useWatcherForLocation)
                {
                    this.LocationService.GetLocationData(watcher.Position.Location, Services.ApplicationSettingsService.GetSetting<int>("ChangeDistance"));
                }
                else
                {
                    this.LocationService.GetLocationData(this._map.BoundingRectangle.Center, Services.ApplicationSettingsService.GetSetting<int>("ChangeDistance"));
                }

                this._useWatcherForLocation = false;

            }

        }

        private void Initialize()
        {

            this._settings = DependencyResolver.Resolve<ShellViewModel>();
            this._settings.PropertyChanged += SettingsChanged;

            this._mapTileSources = new MapTileSourcesViewModel();
            this._mapTileSources.SelectionChanged += MapTileSourceChanged;

            this._map = DependencyResolver.Resolve<Map>();

            if (this._provider == null)
            {

                this._provider = DependencyResolver.Resolve<EntityProvider>();

                if (this._provider != null)
                {
                    ObserveEntities();
                }

            }

            if (this._map != null)
            {

                this._map.CredentialsProvider = new ApplicationIdCredentialsProvider(ADSBingMapsKey);

                if (this._settings.AllowLocation)
                {
                    AddLocationWatcher();
                }

                ObserveMapChangedEvent();

            }
            else
            {
                DependencyResolver.TypeRegistered += TypeRegistered;
            }

        }
   
        private void LocationStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {

            if (this._settings.AllowLocation)
            {

                if (e.Status == GeoPositionStatus.Ready)
                {

                    IGeoPositionWatcher<GeoCoordinate> watcher = DependencyResolver.Resolve<IGeoPositionWatcher<GeoCoordinate>>();

                    if (watcher != null)
                    {

                        this._useWatcherForLocation = true;
                        this._map.SetView(watcher.Position.Location, 11);

                    }

                }

            }

        }

        private void MapTileSourceChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (!this._tileSourceChangedFromProperty)
            {
                base.OnPropertyChanged(x => this.TileSource);
            }

            this._mapViewChangeFromTileSource = true;
            this._tileSourceChangedFromProperty = false;

        }

        private void MapViewChanged(IEvent<MapEventArgs> e)
        {

            bool shouldGetLocationData = false;

            if (!this._mapViewChangeFromTileSource)
            {

                if (this._settings.UpdateOnDistanceChange && this._lastCoordinate != null)
                {

                    if (Mapping.Utilities.Haversine.GetDistance(this._map.BoundingRectangle.Center, this._lastCoordinate, DistanceUnits.Miles) >= this._settings.ChangeDistance)
                    {

                        this._lastCoordinate = this._map.BoundingRectangle.Center;
                        shouldGetLocationData = true;

                    }
 
                }
                else
                {

                    this._lastCoordinate = this._map.BoundingRectangle.Center;
                    shouldGetLocationData = true;

                }

                if (shouldGetLocationData && !this._provider.IsRunning)
                {

                    this._selectedEntity = null;
                    this.Entities.Clear();
                    GetLocationData();

                }

            }

            this._mapViewChangeFromTileSource = false;

        }

        private void ObserveEntities()
        {

            if (this._provider != null)
            {

                this._entityObserver = this._provider.EntityEvents.ObserveOnDispatcher().Subscribe(EntityUpdated);

            }

        }

        private void ObserveMapChangedEvent()
        {

            IObservable<IEvent<MapEventArgs>> mapViewEvents = null;
            IObservable<IEvent<System.Windows.Input.MouseButtonEventArgs>> mapMouseButtonEvents = null;

            if (this._map != null)
            {

                //throttle down this event so we are not asking for new data while a user is scrolling around the map
                mapViewEvents = Observable.FromEvent<MapEventArgs>(e => this._map.ViewChangeEnd += e, e => this._map.ViewChangeEnd -= e).Throttle(TimeSpan.FromMilliseconds(3000)).DistinctUntilChanged();
                this._mapViewObserver = mapViewEvents.ObserveOnDispatcher().Subscribe(MapViewChanged);

                mapMouseButtonEvents = Observable.FromEvent((EventHandler<System.Windows.Input.MouseButtonEventArgs> e) => new System.Windows.Input.MouseButtonEventHandler(e), e => this._map.MouseLeftButtonDown += e, e => this._map.MouseLeftButtonDown -= e).DistinctUntilChanged();
                this._mapPushpinSelectObserver = mapMouseButtonEvents.ObserveOnDispatcher().Subscribe(PushpinSelected);

            }

        }

        private void PushpinSelected(IEvent<System.Windows.Input.MouseButtonEventArgs> e)
        {

            if (e.EventArgs.OriginalSource != null)
            {
                this.SelectedEntity = ((System.Windows.FrameworkElement)e.EventArgs.OriginalSource).DataContext as Entity;
            }

        }

        private void RemoveLocationWatcher()
        {

            IGeoPositionWatcher<GeoCoordinate> watcher = DependencyResolver.Resolve<IGeoPositionWatcher<GeoCoordinate>>();
            watcher.StatusChanged -= LocationStatusChanged;

            if (watcher != null)
            {
                watcher.Stop();
            }

        }

        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (string.Compare(e.PropertyName, "AllowLocation", StringComparison.Ordinal) == 0)
            {

                if (this._settings.AllowLocation)
                {
                    AddLocationWatcher();
                }
                else
                {
                    RemoveLocationWatcher();
                }

            }

        }

        private void TypeRegistered(object sender, TypeRegisteredEventArgs e)
        {

            if (e.Type == typeof(Map) && e.Instance != null)
            {

                DependencyResolver.TypeRegistered -= TypeRegistered;
            
                this._map = (Map)e.Instance;
                this._map.CredentialsProvider = new ApplicationIdCredentialsProvider(ADSBingMapsKey);

                if (this._settings.AllowLocation)
                {
                    AddLocationWatcher();
                }

                ObserveMapChangedEvent();

            }

        }

        #endregion

    }

}
