using System;

namespace Traffix.Mapping.Services
{

    public class GetLocationCompletedEventArgs : EventArgs
    {

        private LocationData _locationData;


        private GetLocationCompletedEventArgs() : base()
        {
        }

        public GetLocationCompletedEventArgs(LocationData locationData) : base()
        {

            if (locationData == null)
            {
                throw new ArgumentNullException("locationData");
            }

            this._locationData = locationData;
        }


        #region public

        public LocationData LocationData
        {

            get
            {
                return this._locationData;
            }

        }

        #endregion

    }

}
