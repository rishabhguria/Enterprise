using System;
using System.Collections;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    [XmlRoot("ThirdPartyFlatFileDetailCollection")]
    public class ThirdPartyFlatFileDetailCollection : System.Collections.ICollection
    {
        // This attribute enables the ArrayList to be serialized:
        [System.Xml.Serialization.XmlArray("ThirdPartyFlatFileDetailCollection")]
        // Explicitly tell the serializer to expect the Item class
        // so it can be properly written to XML from the collection:
        [System.Xml.Serialization.XmlArrayItem("ThirdPartyFlatFileDetail", typeof(ThirdPartyFlatFileDetail))]

        private ArrayList _thirdPartyFlatFileDetailCollection;

        public ThirdPartyFlatFileDetailCollection()
        {
            _thirdPartyFlatFileDetailCollection = new ArrayList();
        }

        public int AddItem(ThirdPartyFlatFileDetail thirdPartyFlatFileDetail)
        {
            return _thirdPartyFlatFileDetailCollection.Add(thirdPartyFlatFileDetail);
        }

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add ThirdPartyFlatFileDetailCollection.IsSynchronized getter implementation
                return _thirdPartyFlatFileDetailCollection.IsSynchronized;
            }
        }


        public int Count
        {
            get
            {
                return _thirdPartyFlatFileDetailCollection.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _thirdPartyFlatFileDetailCollection.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return _thirdPartyFlatFileDetailCollection.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members


        public IEnumerator GetEnumerator()
        {
            return _thirdPartyFlatFileDetailCollection.GetEnumerator();
        }



        public ThirdPartyFlatFileDetail this[int i]
        {
            get
            { return (ThirdPartyFlatFileDetail)_thirdPartyFlatFileDetailCollection[i]; }
        }
        #endregion

        public void Add(ThirdPartyFlatFileDetail thirdPartyFlatFileDetail)
        {
            _thirdPartyFlatFileDetailCollection.Add(thirdPartyFlatFileDetail);
        }


    }
}

