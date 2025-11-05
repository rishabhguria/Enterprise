using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class CACloseData
    {
        private string _caID = string.Empty;

        public string CAID
        {
            get { return _caID; }
            set { _caID = value; }
        }

        private List<ClosingInfo> _caClosingList = new List<ClosingInfo>();

        public List<ClosingInfo> CAClosingList
        {
            get { return _caClosingList; }
            set { _caClosingList = value; }
        }

    }
}
