using Traffix.Core;

namespace Traffix.Mapping.Entities
{
    public class MetadataItem : ObservableObject
    {

        private bool _isReadOnly;
        private string _key;
        private object _value;


        private MetadataItem() : base()
        {
        }

        public MetadataItem(string key, object value) : this(key, value, false)
        {
        }

        public MetadataItem(string key, object value, bool isReadOnly) : base()
        {

            this._isReadOnly = isReadOnly;
            this._key = key;
            this._value = value;

        }


        #region Public

        public bool IsReadOnly
        {

            get
            {
                return this._isReadOnly;
            }

            set
            {

                this._isReadOnly = value;
                base.OnPropertyChanged(x=> this.IsReadOnly);

            }

        }

        public string Key
        {

            get
            {
                return this._key;
            }

        }

        public object Value
        {

            get
            {
                return this._value;
            }

            set
            {

                if (this._isReadOnly)
                {
                    return;
                }

                this._value = value;
                base.OnPropertyChanged(x=> this.Value);

            }

        }

        #endregion

    }

}
