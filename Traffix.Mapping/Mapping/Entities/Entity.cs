using Microsoft.Phone.Controls.Maps.Platform;
using System;
using System.Device.Location;
using Traffix.Core;

namespace Traffix.Mapping.Entities
{

    public class Entity : ObservableObject
    {

        private string _id;
        private Uri _imageSource;
        private double _lifespan;
        private GeoCoordinate _location;
        private readonly MetadataCollection _metadata = new MetadataCollection();
        private string _name;
        private DateTimeOffset _timestamp;


        private Entity() : this(Guid.NewGuid().ToString(), "Entity")
        {
        }

        public Entity(string id) : this(id, "Entity")
        {

        }

        public Entity(string id, string name) : base()
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            this._id = id;
            this._lifespan = 120;
            this._name = name;
            this._timestamp = DateTimeOffset.Now;

        }


        #region public

        public DateTimeOffset CalculatedEndTime
        {

            get
            {
                return this._timestamp.AddSeconds(this._lifespan);
            }

        }

        public string Id
        {

            get
            {
                return this._id;
            }

        }

        public Uri Icon
        {

            get
            {
                return this._imageSource;
            }

            set
            {

                this._imageSource = value;
                base.OnPropertyChanged(x => this.Icon);

            }

        }

        public double Lifespan
        {

            get
            {
                return this._lifespan;
            }

            set
            {

                if (this._lifespan <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this._lifespan = value;
                base.OnPropertyChanged(x => this.Lifespan);

            }

        }

        public GeoCoordinate Location
        {

            get
            {

                if (this._location == null)
                {
                    this._location = new Location();
                }

                return this._location;

            }

            set
            {

                this._location = value;
                base.OnPropertyChanged(x => this.Location);

            }

        }

        public MetadataCollection Metadata
        {

            get
            {
                return this._metadata;
            }

        }

        public string Name
        {

            get
            {
                return this._name;
            }

            set
            {

                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }

                this._name = value;
                base.OnPropertyChanged(x => this.Name);

            }

        }

        public DateTimeOffset Timestamp
        {

            get
            {
                return this._timestamp;
            }

            set
            {

                this._timestamp = value;
                base.OnPropertyChanged(x => this.Timestamp);

            }

        }

        #endregion

    }

}
