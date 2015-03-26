using Newtonsoft.Json.Linq;
using System;
using System.Device.Location;
using System.Net;

namespace Traffix.Mapping.Services
{
    
    public class LocationService
    {

        #region constants

        private const double MilesToMeters = 1609.34;
        private const string ReverseGeocodeUri = "http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?location={0}%2C+{1}&distance={2}&f=pjson";

        #endregion

        private bool _isGettingLocatonData;
        private LocationData _locationData;


        public event EventHandler<GetLocationCompletedEventArgs> GetLocationCompleted;


        public LocationService()
        {
        }


        #region public

        public bool IsGettingLocation
        {

            get
            {
                return this._isGettingLocatonData;
            }

        }

        public LocationData LastLocationData
        {

            get
            {
                return this._locationData;
            }

        }


        public void GetLocationData(GeoCoordinate location, int distance)
        {

            if(location == null)
            {
                throw new ArgumentNullException("location");
            }

            try
            {

                WebClient client = new WebClient();
                //string json = "{location={\"x\":{0},\"y\":={1},\"distance\":{2},\"spatialReference\":{\"wkid\":4326}}}";
                string json = "location={0},{1}&distance={2}&wkid=4356";
                double meters = distance * MilesToMeters;

                client.DownloadStringCompleted += ReverseGeocodeComplete;
                client.DownloadStringAsync(new Uri(string.Format(ReverseGeocodeUri, location.Longitude.ToString(), location.Latitude.ToString(), meters.ToString()), UriKind.RelativeOrAbsolute));
             
                SetIsGettingLocation(true);

            }
            catch (Exception ex)
            {
                SetIsGettingLocation(false);
            }

        }

        #endregion

        #region private


        private void ReverseGeocodeComplete(object sender, DownloadStringCompletedEventArgs e)
        {

            WebClient client = null;
            JObject json = null;
            JObject addressRoot = null;

            try
            {
                client = e.UserState as WebClient;

                if (client != null)
                {
                    client.DownloadStringCompleted -= ReverseGeocodeComplete;
                }
                
                if (e.Error == null & e.Result != null)
                {

                    SetLocationData(new LocationData());

                    json = JObject.Parse(e.Result);
                    addressRoot = json.Value<JObject>("address");

                    this.LastLocationData.Address = addressRoot.Value<string>("Address");
                    this.LastLocationData.City = addressRoot.Value<string>("City");
                    this.LastLocationData.State = addressRoot.Value<string>("Region");
                    this.LastLocationData.Zip = addressRoot.Value<string>("Postal");

                }

                if (GetLocationCompleted != null && this._locationData != null)
                {
                    GetLocationCompleted(this, new GetLocationCompletedEventArgs(this.LastLocationData));
                }  

            }
            catch
            {   
            }
            finally
            {
                SetIsGettingLocation(false);
            }
 
        }

        private void SetLocationData(LocationData locationData)
        {
            this._locationData = locationData;
        }

        private void SetIsGettingLocation(bool gettingLocation)
        {
            this._isGettingLocatonData = gettingLocation;
        }

        #endregion

    }

}
