using System;
using Traffix.Core;

namespace Traffix.Mapping
{

    public class LocationData : ObservableObject
    {

        private string _address;
        private string _city;
        private string _state;
        private string _zip;


        public LocationData() : base()
        {
        }

        public LocationData(string address, string city, string state, string zip)
        {

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException("address");
            }

            if (string.IsNullOrEmpty(city))
            {
                throw new ArgumentNullException("address");
            }

            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException("address");
            }

            if (string.IsNullOrEmpty(zip))
            {
                throw new ArgumentNullException("zip");
            }

            this._address = address;
            this._city = city;
            this._state = state;
            this._zip = zip;

        }


        #region public

        public string Address
        {

            get
            {
                return this._address;
            }

            set
            {

                this._address = value;
                base.OnPropertyChanged(x => this.Address);

            }

        }

        public string City
        {

            get
            {
                return this._city;
            }

            set
            {

                this._city = value;
                base.OnPropertyChanged(x => this.City);

            }

        }

        public string State
        {

            get
            {
                return this._state;
            }

            set
            {

                this._state = value;
                base.OnPropertyChanged(x => this.State);

            }

        }

        public string Zip
        {

            get
            {
                return this._zip;
            }

            set
            {

                this._zip = value;
                base.OnPropertyChanged(x => this.Zip);

            }

        }

        #endregion


    }

}
