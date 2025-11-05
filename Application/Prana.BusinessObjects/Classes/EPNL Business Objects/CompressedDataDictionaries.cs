using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class CompressedDataDictionaries
    {
        private Dictionary<int, ExposurePnlCacheItemList> _outputCompressedData = new Dictionary<int, ExposurePnlCacheItemList>();
        //contains the Compressed data
        public Dictionary<int, ExposurePnlCacheItemList> OutputCompressedData
        {
            get { return _outputCompressedData; }
            set { _outputCompressedData = value; }
        }
    }
}
