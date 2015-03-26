using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using Traffix.Mapping.Entities;
using Traffix.Mapping.Services;

/*
 YAHOO is shutting down their traffic REST API. This class was left as an example.
*/

namespace Traffix.EntityProviders
{

    public class YahooTrafficEntityProvider : EntityProvider
    {

        #region constants

        private const string ProviderName = "Yahoo Traffic Entity Provider";

        //the appkey here is the app key for map quest
        private const string TrafficAddress = "http://local.yahooapis.com/MapsService/V1/trafficData?appid=APP-KEY-HERE&zip={0}";

        #endregion


        private string _lastZip;
        private LocationService _locationService;
        private readonly object _lock = new Object();
        private bool _isRunning;
        private Thread _runThread;
        private bool _stopThread;


        public YahooTrafficEntityProvider() : base(ProviderName)
        {

            this.CanPause = false;

        }


        #region public

        public override bool IsRunning
        {

            get
            {

                lock (this._lock)
                {
                    return this._isRunning;
                }

            }

        }


        public override void Pause()
        {
            return;
        }

        public override void Reset()
        {

            Stop();
            Start();

        }

        public override void Start()
        {

            if (!this.IsRunning)
            {

                this._runThread = new Thread(ThreadRunner);
                this._runThread.IsBackground = true;
                this._runThread.Start();

                SetIsRunning(true);

            }

        }

        public override void Start(params object[] args)
        {

            Start();

        }

        public override void Stop()
        {

            if (this._runThread != null && this._runThread.IsAlive)
            {

                this.StopThread = true;
                SetIsRunning(false);

            }

        }

        #endregion

        #region private

        private LocationService LocationService
        {

            get
            {

                lock (this._lock)
                {
                    return this._locationService;
                }

            }

            set
            {

                lock (this._lock)
                {
                    this._locationService = value;
                }

            }

        }

        private bool StopThread
        {

            get
            {

                lock (this._lock)
                {
                    return this._stopThread;
                }

            }

            set
            {

                lock (this._lock)
                {
                    this._stopThread = value;
                }

            }

        }


        private void DownloadXmlCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            WebClient client = null;

            try
            {
                client = e.UserState as WebClient;

                if (client != null)
                {
                    client.DownloadStringCompleted -= DownloadXmlCompleted;
                }

                if (e.Error == null & e.Result != null)
                {
                    ProcessXml(e.Result);
                }
                else
                {
                    SetIsRunning(false);
                }

            }
            catch (Exception ex)
            {
                SetIsRunning(false);
            }

        }

        private void GetTrafficData()
        {

            WebClient client = null;

            try
            {

                if (this.LocationService != null && this.LocationService.LastLocationData != null && !string.IsNullOrEmpty(this.LocationService.LastLocationData.Zip) && !this.LocationService.LastLocationData.Zip.Equals(this._lastZip, StringComparison.OrdinalIgnoreCase))
                {

                    client = new WebClient();
                    this._lastZip = this.LocationService.LastLocationData.Zip;

                    client.DownloadStringCompleted += DownloadXmlCompleted;
                    client.DownloadStringAsync(new Uri(string.Format(TrafficAddress, this.LocationService.LastLocationData.Zip), UriKind.Absolute), client);

                }
                else
                {
                    SetIsRunning(false);
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                SetIsRunning(false);
            }

        }

        private void ProcessXml(string xml)
        {

            IEnumerable<XElement> results = null;
            XElement element = null;
            Entity ent = null;
            string resultType;
            XNamespace ns = null;

            try
            {

                element = XElement.Parse(xml);
                results = element.Elements(element.GetDefaultNamespace() + "Result");

                foreach (XElement elem in results)
                {

                    ns = elem.GetDefaultNamespace();
                    resultType = elem.Attribute("type").Value;

                    ent = new Entity(Guid.NewGuid().ToString());
                    ent.Location.Latitude = double.Parse(elem.Element(ns + "Latitude").Value);
                    ent.Location.Longitude = double.Parse(elem.Element(ns + "Longitude").Value);

                    if (resultType.Equals("incident", StringComparison.OrdinalIgnoreCase))
                    {
                        ent.Icon = new Uri("/Resources/Images/Incident.png", UriKind.Relative);
                    }
                    else if (resultType.Equals("construction", StringComparison.OrdinalIgnoreCase))
                    {
                        ent.Icon = new Uri("/Resources/Images/Construction.png", UriKind.Relative);
                    }
                    else
                    {
                        ent.Icon = new Uri("/Resources/Images/Incident.png", UriKind.Relative);
                    }

                    ent.Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(double.Parse(elem.Element(ns + "ReportDate").Value)).ToLocalTime();
                    ent.Lifespan = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(double.Parse(elem.Element(ns + "EndDate").Value)).Subtract(ent.Timestamp.DateTime).TotalSeconds;

                    ent.Metadata.Add("Title", elem.Element(ns + "Title").Value);
                    ent.Metadata.Add("Description", elem.Element(ns + "Description").Value, true);

                    UpdateEntity(ent, EntityUpdateType.Updated);

                    if (this.StopThread)
                    {
                        break;
                    }

                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                SetIsRunning(false);
            }
        
        }

        private void SetIsRunning(bool isRunning)
        {

            lock (this._lock)
            {
                this._isRunning = isRunning;
            }

        }

        private void ThreadRunner(object state)
        {

            if (this.LocationService == null)
            {
                this.LocationService = DependencyResolver.Resolve<LocationService>();           
            }

            GetTrafficData();

        }

        #endregion

    }

}
