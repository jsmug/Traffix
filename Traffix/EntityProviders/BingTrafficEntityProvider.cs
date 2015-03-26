using Microsoft.Phone.Controls.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using Traffix.Mapping.Entities;
using Traffix.Mapping.Services;

namespace Traffix.EntityProviders
{

    public class BingTrafficEntityProvider : EntityProvider
    {

        #region constants

        private const string ProviderName = "Bing Traffic Entity Provider";

        //the appkey here is the app key for bing
        //you can get an appkey at https://www.bingmapsportal.com/
        private const string TrafficAddress = "http://dev.virtualearth.net/REST/v1/Traffic/Incidents/{0},{1},{2},{3}?output=xml&key=APP-KEY-HERE";

        #endregion


        private string _lastZip;
        private LocationService _locationService;
        private readonly object _lock = new Object();
        private bool _isRunning;
        private Thread _runThread;
        private bool _stopThread;


        public BingTrafficEntityProvider() : base(ProviderName)
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
            Map map = DependencyResolver.Resolve<Map>();

            try
            {

                if (map != null && this.LocationService != null && this.LocationService.LastLocationData != null && !string.IsNullOrEmpty(this.LocationService.LastLocationData.Zip) && !this.LocationService.LastLocationData.Zip.Equals(this._lastZip, StringComparison.OrdinalIgnoreCase))
                {

                    client = new WebClient();
                    this._lastZip = this.LocationService.LastLocationData.Zip;

                    client.DownloadStringCompleted += DownloadXmlCompleted;
                    client.DownloadStringAsync(new Uri(string.Format(TrafficAddress, map.BoundingRectangle.South, map.BoundingRectangle.West, map.BoundingRectangle.North, map.BoundingRectangle.East), UriKind.Absolute), client);

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

            try
            {

                element = RemoveXmlNamespace(XElement.Parse(xml));
                results = element.Element("ResourceSets").Element("ResourceSet").Element("Resources").Elements();

                foreach (XElement elem in results)
                {

                    ent = new Entity(elem.Element("IncidentId").Value);
                    ent.Location.Latitude = double.Parse(elem.Element("Point").Element("Latitude").Value);
                    ent.Location.Longitude = double.Parse(elem.Element("Point").Element("Longitude").Value);

                    resultType = elem.Element("Type").Value;

                    if (resultType.Equals("Construction", StringComparison.OrdinalIgnoreCase))
                    {
                        ent.Icon = new Uri("/Resources/Images/Construction.png", UriKind.Relative);
                    }
                    else
                    {
                        ent.Icon = new Uri("/Resources/Images/Incident.png", UriKind.Relative);
                    }

                    ent.Timestamp = DateTimeOffset.Parse(elem.Element("StartTimeUTC").Value).ToLocalTime();
                    ent.Lifespan = DateTimeOffset.Parse(elem.Element("EndTimeUTC").Value).ToLocalTime().Subtract(ent.Timestamp).TotalSeconds;

                    ent.Metadata.Add("ShortDescription", elem.Element("Description").Value, true);
                    ent.Metadata.Add("LongDescription", elem.Element("Description").Value, true);

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

        private XElement RemoveXmlNamespace(XElement root)
        {
            return new XElement(root.Name.LocalName, root.HasElements ? root.Elements().Select(el => RemoveXmlNamespace(el)) : (object)root.Value);
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
