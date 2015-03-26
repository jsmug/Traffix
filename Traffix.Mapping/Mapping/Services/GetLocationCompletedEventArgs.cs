using System;

namespace Traffix.Mapping.Services
{

    public class GetLocationCompletedEventArgs : EventArgs
    {

        private LocationData baseLocationData;


        private GetLocationCompletedEventArgs() : base()
        {
        }

        public GetLocationCompletedEventArgs(LocationData locationData) : base()
        {

            if (locationData == null)
            {
                throw new ArgumentNullException("locationData");
            }

            this.baseLocationData = locationData;
        }


        #region public

        public LocationData LocationData
        {

            get
            {
                return this.baseLocationData;
            }

        }

        #endregion

    }

}
