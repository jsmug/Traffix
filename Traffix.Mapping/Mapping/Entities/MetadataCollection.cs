using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Traffix.Mapping.Entities
{

    public class MetadataCollection : Collection<MetadataItem>
    {

        public MetadataCollection() : base()
        {
        }

        public MetadataCollection(IEnumerable<MetadataItem> collection) : this(collection.ToList())
        {
        }

        public MetadataCollection(List<MetadataItem> list) : base(list)
        {
        }


        #region Public

        public MetadataItem this[string key]
        {

            get
            {

                return (from MetadataItem i in base.Items where (string.Compare(i.Key, key, StringComparison.OrdinalIgnoreCase) == 0) select i).FirstOrDefault();

            }

        }


        public void Add(string key, object value)
        {

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            base.Add(new MetadataItem(key, value));

        }

        public void Add(string key, object value, bool isReadOnly)
        {

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            base.Add(new MetadataItem(key, value, isReadOnly));

        }

        public void Remove(string key)
        {

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            MetadataItem item = (from MetadataItem i in base.Items where (string.Compare(i.Key, key, StringComparison.OrdinalIgnoreCase) == 0) select i).FirstOrDefault();

            if (item != null)
            {
                base.Items.Remove(item);
            }

        }

        #endregion

    }

}
