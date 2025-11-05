using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class SecMasterUpdateDataByImportList : List<SecMasterUpdateDataByImportUI>
    {
        public SecMasterUpdateDataByImportList()
        {

        }
        private string _requestID;

        public string RequestID
        {
            get { return _requestID; }
            set { _requestID = value; }
        }
        private string _userID;

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

    }
}
