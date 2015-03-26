using System;
using System.Device.Location;
using System.Net;
using System.Threading;

namespace Traffix.Mapping.Services
{

    public class MockGeoCoordinateWatcherService : IGeoPositionWatcher<GeoCoordinate>
    {

        #region constants

        //you can get and api key here http://ipinfodb.com/register.php
        private const string IPInfoLookup = "http://api.ipinfodb.com/v3/ip-city/?key=APP-KEY-HERE";
        private const int WiredIanaInterfaceType = 6;
        private const int WirelessIanaInterfaceType = 71;

        #endregion


        private string _ipAddress;
        private GeoPosition<GeoCoordinate> _position;
        private GeoPositionStatus _status = GeoPositionStatus.Disabled;
        private Timer _timer = null;


        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;
        public event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;


        public MockGeoCoordinateWatcherService()
        {

            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }

        }


        #region public
 
        public GeoPosition<GeoCoordinate> Position
        {
            
            get 
            {
                return this._position;
            }

        }

        public GeoPositionStatus Status
        {

            get 
            {
                return this._status;
            }

        }

        public void Start(bool suppressPermissionPrompt)
        {

            if (this._timer != null)
            {
                return;
            }

            this._timer = new Timer(TimerFire, SynchronizationContext.Current, 0, 600000);

        }

        public void Start()
        {
            Start(true);
        }

        public void Stop()
        {

            if (this._timer != null)
            {

                this._timer.Change(Timeout.Infinite, Timeout.Infinite);
                this._timer.Dispose();

                this._timer = null;

            }
           
        }

        public bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout)
        {
            
            Start(true);
            return true;

        }

        #endregion

        #region private

        private string IPAddress
        {

            get
            {
                return this._ipAddress;
            }

            set
            {
                this._ipAddress = value;
            }

        }

        private void GetLocation(object state)
        {

            this.IPAddress = default(string);
            SetStatus(GeoPositionStatus.Initializing);

            if (StatusChanged != null)
            {
                StatusChanged(this, new GeoPositionStatusChangedEventArgs(this.Status));
            }

            WebClient client = new WebClient();

            client.DownloadStringCompleted += GetLocationFromIpAddressCompleted;
            client.DownloadStringAsync(new Uri(IPInfoLookup, UriKind.RelativeOrAbsolute));

        }

        private void GetLocationFromIpAddressCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            string[] data = null;
            double latitude = default(double);
            double longitude = default(double);

            if (e.Error == null && !string.IsNullOrEmpty(e.Result))
            {
            
                data = e.Result.Split(';');

                if (data.Length == 11)
                {

                    double.TryParse(data[8], out latitude);
                    double.TryParse(data[9], out longitude);

                    SetPosition(new GeoPosition<GeoCoordinate>());        
                    this.Position.Location = new GeoCoordinate(latitude, longitude);
                    this.Position.Timestamp = DateTimeOffset.Now;

                    SetStatus(GeoPositionStatus.Ready);

                }
                else
                {
                    SetStatus(GeoPositionStatus.NoData);
                }

            }
            else
            {
                SetStatus(GeoPositionStatus.NoData);
            }

            if (StatusChanged != null)
            {
                StatusChanged(this, new GeoPositionStatusChangedEventArgs(this.Status));
            }

            if (this.Status != GeoPositionStatus.NoData)
            {

                if (PositionChanged != null)
                {
                    PositionChanged(this, new GeoPositionChangedEventArgs<GeoCoordinate>(this.Position));
                }

            }

        }

        private void SetStatus(GeoPositionStatus status)
        {
            this._status = status;
        }

        private void SetPosition(GeoPosition<GeoCoordinate> position)
        {
            this._position = position;
        }

        private void TimerFire(object state)
        {

            SynchronizationContext context = state as SynchronizationContext;

            if (context != null)
            {
                context.Post(new SendOrPostCallback(GetLocation), null);
            }

        }

        #endregion

    }

}
