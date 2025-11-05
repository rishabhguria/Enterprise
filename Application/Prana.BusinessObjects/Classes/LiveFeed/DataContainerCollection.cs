using System.Collections;
using System.Collections.Generic;

namespace Prana.BusinessObjects.LiveFeed
{
    public class DataContainerCollection
    {
        private const int MAX_NO_ARRAYLIST = 5;
        static int _currentCollectionIndex = 1;
        private static DataContainerCollection _dataCollection = null;
        static Dictionary<int, DataContainer> _dataContainers = new Dictionary<int, DataContainer>();

        static DataContainerCollection()
        {
            _dataCollection = new DataContainerCollection();
        }
        public static DataContainerCollection GetInstance()
        {

            return _dataCollection;
        }

        private delegate void UpdateDataContainerHandler(object[] objArray);
        //public void AddRecordAsync(object[] objArray)
        //{
        //    UpdateDataContainerHandler dataHandler = new UpdateDataContainerHandler(AddRecord);
        //    dataHandler.BeginInvoke(objArray, null, null);
        //}

        public void AddRecord(object[] objArray)
        {
            if (_currentCollectionIndex % MAX_NO_ARRAYLIST == 0)
            {
                _currentCollectionIndex = 1;
            }
            if (!_dataContainers.ContainsKey(_currentCollectionIndex))
            {
                _dataContainers[_currentCollectionIndex] = new DataContainer();
            }
            _dataContainers[_currentCollectionIndex].AddRecord(objArray);
        }

        public Dictionary<string, Hashtable> FetchLatestContainer()
        {
            if (_dataContainers.ContainsKey(_currentCollectionIndex))
            {
                return _dataContainers[_currentCollectionIndex++].HashSymbolArraylistCollection;
            }
            return null;
        }
    }

    public class DataContainer
    {
        Dictionary<string, Hashtable> _symbolArraylistHash = new Dictionary<string, Hashtable>();
        //private DataContainer()
        //{ 
        //}
        public void AddRecord(object[] objArray)
        {
            string symbol = objArray[LiveFeedConstants.iINDEX_SYMBOL].ToString();
            string mmid = objArray[LiveFeedConstants.iINDEX_MMID].ToString();
            if (!_symbolArraylistHash.ContainsKey(symbol))
            {
                //TODO: Put a check here. If the mmid already exists then update else add.
                _symbolArraylistHash.Add(symbol, new Hashtable());
            }
            Hashtable list = _symbolArraylistHash[symbol];

            //_symbolArraylistHash[symbol].Add(objArray);
            if (list.ContainsKey(mmid))
            {
                list[mmid] = objArray;
            }
            else
            {
                list.Add(mmid, objArray);
            }



        }
        public Dictionary<string, Hashtable> HashSymbolArraylistCollection
        {
            get { return _symbolArraylistHash; }
        }
    }
}
