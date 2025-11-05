using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ThirdPartyFlatFileHeader
    {
        private Int64 _uniqueRefID = 0;
        public ThirdPartyFlatFileHeader()
        {
            _uniqueRefID = Int64.Parse(DateTime.Now.ToString("MMddHHmmss"));
        }
        private string _tranfRefNumber = string.Empty;
        /// <summary>
        /// it is a unique system generated number
        /// </summary> 
        public string TranfRefNumber
        {
            get
            {
                return _uniqueRefID.ToString();
            }
            set
            {
                _tranfRefNumber = value;
            }
        }

        private string _date = string.Empty;

        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private string _dateAndTime = string.Empty;

        public string DateAndTime
        {
            get { return _dateAndTime; }
            set { _dateAndTime = value; }
        }

        private int _recordCount = 0;

        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

    }
}
